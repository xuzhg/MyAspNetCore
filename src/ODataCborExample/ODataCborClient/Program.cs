// See https://aka.ms/new-console-template for more information
using ODataCborClient;



ODataClient client = new ODataClient(@"http://localhost:5015/odata/");

await client.ListBooks();

await client.ListBooks("application/cbor");

Console.WriteLine("Press any key to quit!");
Console.ReadKey();