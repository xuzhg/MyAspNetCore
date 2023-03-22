using Bookstores;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace gRPC.OData.Client
{
    internal class ODataBookstoreClient : IBookStoreClient
    {
        private readonly string _baseUri;

        public ODataBookstoreClient(string baseUri)
        {
            _baseUri = baseUri;
        }

        public async Task ListShelves()
        {
            Console.WriteLine($"\nOData: List Shelves:");

            string requestUri = $"{_baseUri}/odata/shelves";
            using var client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
            var response = await client.GetAsync(requestUri);
            Console.WriteLine("--Status code: " + response.StatusCode.ToString());
            string body = await response.Content.ReadAsStringAsync();
            Console.WriteLine("--Response body:");
            Console.WriteLine(BeautifyJson(body));
            Console.WriteLine();
        }

        public async Task<Shelf> CreateShelf(Shelf shelf)
        {
            Console.WriteLine($"OData: Create shelf:");

            string requestUri = $"{_baseUri}/odata/shelves";

            using var client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Version= HttpVersion.Version20,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
            };
            request.Content = new StringContent(JsonSerializer.Serialize(shelf));
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await client.SendAsync(request);
            Console.WriteLine("--Status code: " + response.StatusCode.ToString());
            string body = await response.Content.ReadAsStringAsync();

            Shelf bodyShelf = JsonSerializer.Deserialize<Shelf>(body);

            Console.WriteLine("--Response body:");
            Console.WriteLine(BeautifyJson(body));
            Console.WriteLine();
            return bodyShelf;
        }

        public async Task GetShelf(long shelfId)
        {
            Console.WriteLine($"OData: Get shelf at '{shelfId}':");

            string requestUri = $"{_baseUri}/odata/shelves/{shelfId}";
            using var client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
            var response = await client.GetAsync(requestUri);
            Console.WriteLine("--Status code: " + response.StatusCode.ToString());
            string body = await response.Content.ReadAsStringAsync();
            Console.WriteLine("--Response body:");
            Console.WriteLine(BeautifyJson(body));
            Console.WriteLine();
        }

        public async Task DeleteShelf(long shelfId)
        {
            Console.WriteLine($"OData: Delete shelf at '{shelfId}':");

            string requestUri = $"{_baseUri}/odata/shelves/{shelfId}";
            using var client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
            var response = await client.DeleteAsync(requestUri);
            Console.WriteLine("--Status code: " + response.StatusCode.ToString());
            Console.WriteLine();
        }

        public async Task ListBooks(long shelfId)
        {
            Console.WriteLine($"\nOData: List books at shelf '{shelfId}':");

            string requestUri = $"{_baseUri}/odata/shelves/{shelfId}/books";
            using var client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
            var response = await client.GetAsync(requestUri);
            Console.WriteLine("--Status code: " + response.StatusCode.ToString());
            string body = await response.Content.ReadAsStringAsync();
            Console.WriteLine("--Response body:");
            Console.WriteLine(BeautifyJson(body));
            Console.WriteLine();
        }

        public async Task<Book> CreateBook(long shelfId, Book book)
        {
            Console.WriteLine($"OData: Create book for shelf '{shelfId}':");

            string requestUri = $"{_baseUri}/odata/shelves/{shelfId}/books";

            using var client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Version= HttpVersion.Version20,
                VersionPolicy = HttpVersionPolicy.RequestVersionOrHigher,
            };
            request.Content = new StringContent(JsonSerializer.Serialize(book));
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            var response = await client.SendAsync(request);
            Console.WriteLine("--Status code: " + response.StatusCode.ToString());
            string body = await response.Content.ReadAsStringAsync();
            Console.WriteLine("--Response body:");

            Book bodyBook = JsonSerializer.Deserialize<Book>(body);
            Console.WriteLine(BeautifyJson(body));
            Console.WriteLine();
            return bodyBook;
        }

        public async Task GetBook(long shelfId, long bookId)
        {
            Console.WriteLine($"OData: Get book '{bookId}' from shelf '{shelfId}':");

            string requestUri = $"{_baseUri}/odata/shelves/{shelfId}/books/{bookId}";
            using var client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
            var response = await client.GetAsync(requestUri);
            Console.WriteLine("--Status code: " + response.StatusCode.ToString());
            string body = await response.Content.ReadAsStringAsync();
            Console.WriteLine("--Response body:");
            Console.WriteLine(BeautifyJson(body));
            Console.WriteLine();
        }

        public async Task DeleteBook(long shelfId, long bookId)
        {
            Console.WriteLine($"OData: Delete book '{bookId}' from shelf '{shelfId}':");

            string requestUri = $"{_baseUri}/odata/shelves/{shelfId}/books/{bookId}";
            using var client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrHigher
            };
            var response = await client.DeleteAsync(requestUri);
            Console.WriteLine("--Status code: " + response.StatusCode.ToString());
            Console.WriteLine();
        }

        static T Deserialize<T>(string payload)
        {
            return JsonSerializer.Deserialize<T>(payload);
        }

        static string BeautifyJson(string json)
        {
            using var jDoc = JsonDocument.Parse(json);
            return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
