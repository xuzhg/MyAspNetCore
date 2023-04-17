// See https://aka.ms/new-console-template for more information
using ODataProtobuf.Client;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

string baseUri = @"https://localhost:5024";

await ListShelves(baseUri);

//GrpcBookstoreClient client = new GrpcBookstoreClient($"{baseUri}/odata");

//await client.PlayCURD();

Console.WriteLine("Hello, World!");



async Task ListShelves(string baseUri)
{
    Console.WriteLine($"\nOData: List Shelves:");

    string requestUri = $"{baseUri}/odata/shelves";
    Console.WriteLine(requestUri);
    using var client = new HttpClient
    {
        DefaultRequestVersion = HttpVersion.Version20,
        DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
    };

    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-protobuf"));

    var response = await client.GetAsync(requestUri);
    Console.WriteLine("--Status code: " + response.StatusCode.ToString());
    byte[] body = await response.Content.ReadAsByteArrayAsync();
    Console.WriteLine("--Response body:");
    //Console.WriteLine(body);
    Console.WriteLine(Convert.ToBase64String(body));
    // Console.WriteLine(BeautifyJson(body));
    Console.WriteLine();
}

static string BeautifyJson(string json)
{
    using var jDoc = JsonDocument.Parse(json);
    return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
}