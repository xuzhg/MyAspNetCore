// See https://aka.ms/new-console-template for more information
using ODataCborClient;



ODataClient client = new ODataClient(@"http://localhost:5015/odata/");

Console.WriteLine("----------- List books by default!");

await client.ListBooks();

Console.WriteLine("----------- Press any key to list books using CBOR!");
Console.ReadKey();

await client.ListBooks("application/cbor");

Console.WriteLine("Press any key to quit!");
Console.ReadKey();