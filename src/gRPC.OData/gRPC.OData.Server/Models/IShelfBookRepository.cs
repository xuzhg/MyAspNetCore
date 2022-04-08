using Bookstores;

namespace gRPC.OData.Server.Models
{
    public interface IShelfBookRepository
    {
        IEnumerable<Shelf> GetShelves();

        Shelf CreateShelf(Shelf shelf);

        Shelf GetShelf(long shelfId);

        void DeleteShelf(long shelfId);

        IEnumerable<Book> GetBooks(long shelfId);

        Book CreateBook(long shelfId, Book book);

        Book GetBook(long shelfId, long bookId);

        void DeleteBook(long shelfId, long bookId);
    }
}
