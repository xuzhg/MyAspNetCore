using Default;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData;
using Microsoft.OData.Client;
using Microsoft.OData.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Formats.Cbor;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ODataCborClient;

public class MyHttpWebResponseMessage : HttpWebResponseMessage, IContainerProvider
{
    public MyHttpWebResponseMessage(HttpWebResponseMessage responseMessage)
        : base (GetHeaders(responseMessage),
                (int)responseMessage.StatusCode,
                () => GetMyStream(responseMessage))
    {
        IServiceCollection services = new ServiceCollection();
        Container = services.BuildServiceProvider();

    }

    public static IDictionary<string, string> GetHeaders(HttpWebResponseMessage responseMessage)
    {
        Console.WriteLine("In MyHttpWebResponseMessage Content-Type: " + responseMessage.GetHeader("Content-Type"));

        IDictionary<string, string> headers = new Dictionary<string, string>();
        foreach (var header in responseMessage.Headers)
        {
            if (header.Key == "Content-Type" && header.Value.Contains("application/cbor"))
            {
                headers["Content-Type"] = "application/json";
            }
            else
            {
                headers[header.Key] = header.Value;
            }
        }

        return headers;
    }

    public IServiceProvider Container
    {
        // QueryResult.CreateMaterializer() create HttpWebResponseMessage again?
        // So, we can't use it.
        get;
    }

    public static Stream GetMyStream(HttpWebResponseMessage responseMessage)
    {
        Stream stream = responseMessage.GetStream();
        string contentType = responseMessage.GetHeader("Content-Type");
        if (contentType != null && contentType.Contains("application/cbor"))
        {
            MemoryStream memoryStream = new MemoryStream();

            ConvertToPlainJson(stream, memoryStream);

            memoryStream.Position = 0;

            return memoryStream;
        }

        return stream;
    }

    private static void ConvertToPlainJson(Stream source, MemoryStream destination)
    {
        byte[] data = ReadAllBytes(source);
        Print(data);
        CborReader cborReader = new CborReader(new ReadOnlyMemory<byte>(data));

        Utf8JsonWriter jsonWriter = new Utf8JsonWriter(destination);
        Stack<JsonNodeType> scopes = new Stack<JsonNodeType>();

        CborReaderState readerState = cborReader.PeekState();
        while (readerState != CborReaderState.Finished)
        {
            switch (readerState)
            {
                case CborReaderState.StartArray:
                    scopes.Push(JsonNodeType.StartArray);
                    cborReader.ReadStartArray();
                    jsonWriter.WriteStartArray();
                    break;
                case CborReaderState.EndArray:
                    scopes.Pop();
                    cborReader.ReadEndArray();
                    jsonWriter.WriteEndArray();
                    break;
                case CborReaderState.StartMap:
                    scopes.Push(JsonNodeType.StartObject);
                    cborReader.ReadStartMap();
                    jsonWriter.WriteStartObject();
                    break;
                case CborReaderState.EndMap:
                    scopes.Pop();
                    cborReader.ReadEndMap();
                    jsonWriter.WriteEndObject();
                    break;

                case CborReaderState.TextString:

                    string value = cborReader.ReadTextString();
                    if (IsInObject(scopes))
                    {
                        jsonWriter.WritePropertyName(value);
                        scopes.Push(JsonNodeType.Property);
                    }
                    else if (IsInProperty(scopes))
                    {
                        jsonWriter.WriteStringValue(value);
                        scopes.Pop();
                    }
                    else
                    {
                        jsonWriter.WriteStringValue(value);
                    }

                    break;

                case CborReaderState.UnsignedInteger:
                    uint uInt32 = cborReader.ReadUInt32();

                    if (IsInObject(scopes))
                    {
                        throw new InvalidOperationException("NOOO");
                    }
                    else if (IsInProperty(scopes))
                    {
                        jsonWriter.WriteNumberValue(uInt32);
                        scopes.Pop();
                    }
                    else
                    {
                        jsonWriter.WriteNumberValue(uInt32);
                    }

                    break;

                default:
                    throw new NotImplementedException($"Not implemented, please add more case to handle {readerState}");
            }

            readerState = cborReader.PeekState();
        }

        jsonWriter.Flush();

      //  string json = Encoding.UTF8.GetString(destination.ToArray());
      //  Console.WriteLine(json);
    }

    private static void Print(byte[] data)
    {
        string hex = BitConverter.ToString(data).Replace("-", " ");
        Console.WriteLine(hex);
    }

    private static bool IsInObject(Stack<JsonNodeType> scopes) => scopes.Count > 0 && scopes.Peek() == JsonNodeType.StartObject;

    private static bool IsInProperty(Stack<JsonNodeType> scopes) => scopes.Count > 0 && scopes.Peek() == JsonNodeType.Property;

    private static byte[] ReadAllBytes(Stream instream)
    {
        if (instream is MemoryStream)
            return ((MemoryStream)instream).ToArray();

        using (var memoryStream = new MemoryStream())
        {
            instream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}

public class MyHttpClientRequestMessage : HttpClientRequestMessage
{
    public MyHttpClientRequestMessage(DataServiceClientRequestMessageArgs args)
        : base(args)
    {

    }

    public override IAsyncResult BeginGetResponse(AsyncCallback callback, object state)
    {
        return base.BeginGetResponse(callback, state);
    }

    /// <summary>
    /// Ends an asynchronous request to an Internet resource.
    /// </summary>
    /// <param name="asyncResult">The pending request for a response.</param>
    /// <returns>A System.Net.HttpResponseMessage that contains the response from the Internet resource.</returns>
    public override IODataResponseMessage EndGetResponse(IAsyncResult asyncResult)
    {
        IODataResponseMessage responseMessage = base.EndGetResponse(asyncResult);

        HttpWebResponseMessage httpWebResponseMessage = responseMessage as HttpWebResponseMessage;

        return new MyHttpWebResponseMessage(httpWebResponseMessage);

    }

    public override IODataResponseMessage GetResponse()
    {// not used, never call here
        IODataResponseMessage responseMessage = base.GetResponse();

        HttpWebResponseMessage httpWebResponseMessage = responseMessage as HttpWebResponseMessage;

        return new MyHttpWebResponseMessage(httpWebResponseMessage);
    }
}

internal class ODataClient
{
    private readonly string _baseUri;
    public ODataClient(string baseUri)
    {
        _baseUri = baseUri;
    }

    public async Task ListBooks(string acceptHeader = null)
    {
        Console.WriteLine($"\nOData: List books':");

        Container container = new Container(new Uri(_baseUri));

        container.ReceivingResponse += (sender, eventArgs) =>
        {
            Console.WriteLine("ReceivingResponse Content-Type:" + eventArgs.ResponseMessage.GetHeader("Content-Type"));
        };

        //       container.Format.ODataFormat.
        container.Configurations.RequestPipeline.OnMessageCreating = (args) =>
        {
            var message = new MyHttpClientRequestMessage(args);

            if (acceptHeader != null)
            {
                message.SetHeader("Accept", acceptHeader);
            }

            string requestAcceptHeader = message.GetHeader("Accept");
            Console.WriteLine($"Request Header Accept:{requestAcceptHeader}");

            return message;
        };

      //  container.Configurations.ResponsePipeline.On

        //container.Configurations.ResponsePipeline.OnMessageReaderSettingsCreated(args =>
        //{
        //    // args.Settings.
        //});

        //container.Configurations.ResponsePipeline.OnEntityMaterialized(args =>
        //{
        //    //args.
        //});

        var books = await container.Books.ExecuteAsync();
        foreach (var book in books)
        {
            Console.WriteLine($" {book.Id}) Title='{book.Title}', Author='{book.Author}', ");
        }

        Console.WriteLine();
    }
}
