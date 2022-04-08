using Bookstores;
using Google.Protobuf.WellKnownTypes;
using gRPC.OData.Server.Models;
using Grpc.Core;

namespace gRPC.OData.Server.Services
{
    /// <summary>
    /// The bookstore gRPC service implementation.
    /// </summary>
    public class BookstoreService : Bookstore.BookstoreBase
    {
        private readonly ILogger _logger;
        private readonly IShelfBookRepository _shelfBookRepository;

        public BookstoreService(ILoggerFactory loggerFactory, IShelfBookRepository shelfBookRepository)
        {
            _logger = loggerFactory.CreateLogger<BookstoreService>();
            _shelfBookRepository = shelfBookRepository;
        }

        // list shelves
        public override Task<ListShelvesResponse> ListShelves(Empty request, ServerCallContext context)
        {
            IEnumerable<Shelf> shelves = _shelfBookRepository.GetShelves();
            ListShelvesResponse response = new ListShelvesResponse();
            foreach (var shelf in shelves)
            {
                response.Shelves.Add(shelf);
            }

            return Task.FromResult(response);
        }

        // create shelf
        public override Task<Shelf> CreateShelf(CreateShelfRequest request, ServerCallContext context)
        {
            Shelf shelf = _shelfBookRepository.CreateShelf(request.Shelf);
            return Task.FromResult(shelf);
        }

        // get shelf
        public override Task<Shelf> GetShelf(GetShelfRequest request, ServerCallContext context)
        {
            Shelf shelf = _shelfBookRepository.GetShelf(request.Shelf);

            return Task.FromResult(shelf);
        }

        // delete shelf
        public override Task<Empty> DeleteShelf(DeleteShelfRequest request, ServerCallContext context)
        {
            _shelfBookRepository.DeleteShelf(request.Shelf);
            return Task.FromResult(new Empty());
        }

        // list the books
        public override Task<ListBooksResponse> ListBooks(ListBooksRequest request, ServerCallContext context)
        {
            IEnumerable<Book> books = _shelfBookRepository.GetBooks(request.Shelf);

            ListBooksResponse response = new ListBooksResponse();
            foreach (Book book in books)
            {
                response.Books.Add(book);
            };

            return Task.FromResult(response);
        }

        // create book
        public override Task<Book> CreateBook(CreateBookRequest request, ServerCallContext context)
        {
            Book book = _shelfBookRepository.CreateBook(request.Shelf, request.Book);

            return Task.FromResult(book);
        }

        // get book
        public override Task<Book> GetBook(GetBookRequest request, ServerCallContext context)
        {
            Book book = _shelfBookRepository.GetBook(request.Shelf, request.Book);

            return Task.FromResult(book);
        }

        // delete book
        public override Task<Empty> DeleteBook(DeleteBookRequest request, ServerCallContext context)
        {
            _shelfBookRepository.DeleteBook(request.Shelf, request.Book);
            return Task.FromResult(new Empty());
        }
    }
}
