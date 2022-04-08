using Bookstores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gRPC.OData.Client
{
    public interface IBookStoreClient
    {
        Task ListShelves();

        Task<Shelf> CreateShelf(Shelf shelf);

        Task GetShelf(long shelfId);

        Task DeleteShelf(long shelfId);

        Task ListBooks(long shelfId);

        Task<Book> CreateBook(long shelfId, Book book);

        Task GetBook(long shelfId, long bookId);

        Task DeleteBook(long shelfId, long bookId);
    }
}
