
using Bookstores;
using gRPC.OData.Client;

string baseUri = "https://localhost:7260";

IBookStoreClient gRPCbookStore = new GrpcBookstoreClient(baseUri);
IBookStoreClient oDataBookStore = new ODataBookstoreClient(baseUri);

while (true)
{
    Console.Clear();
    Console.WriteLine("What do you want to do?");
    Console.WriteLine("    a) Enter 1 for gRPC.");
    Console.WriteLine("    b) Enter 2 for OData.");
    Console.WriteLine("    c) q/Q exit!");

    IBookStoreClient client = null;
    string input = Console.ReadLine();
    Console.Clear();
    switch (input)
    {
        case "1":
            client = gRPCbookStore;
            break;

        case "2":
            client = oDataBookStore;
            break;

        case "Q":
        case "q":
            return;

        default:
            Console.WriteLine("Wrong input!");
            break;
    }

    if (client != null)
    {
        await PlayCURD(client);
    }

    Console.WriteLine("\nPress any key to continue....");
    Console.ReadKey();
}

static async Task PlayCURD(IBookStoreClient client)
{
    // List the shelves
    await client.ListShelves();

    // create a new shelves
    Shelf shelf = new Shelf
    {
        Id = 0,
        Theme = "Poetry"
    };
    shelf = await client.CreateShelf(shelf);

    // create book #1 to new shelf
    Book book1 = new Book { Id = 0, Author = "The Night Before Easter", Title = "Natasha Wing" };
    await client.CreateBook(shelf.Id, book1);

    // create book #2 to new shelf
    Book book2 = new Book { Id = 0, Author = "Milk and Honey", Title = "Rupi Kaur " };
    await client.CreateBook(shelf.Id, book2);

    // List books
    await client.ListBooks(shelf.Id);

    // List shelves
    await client.ListShelves();

    // Delete shelve
    await client.DeleteShelf(shelf.Id);

    // List shelves again
    await client.ListShelves();
}
