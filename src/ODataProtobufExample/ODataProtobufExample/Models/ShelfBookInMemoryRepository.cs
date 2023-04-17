using Bookstores;

namespace ODataProtobufExample.Models;

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

public class ShelfBookInMemoryRepository : IShelfBookRepository
{
    #region Sample Data
    public static IList<Shelf> _sheves;

    static ShelfBookInMemoryRepository()
    {
        IList<Book> fictions = new List<Book>
        {
            new Book { Id = 11, Title = "西游记" , Author = "吳承恩" },
            new Book { Id = 12, Title = "The Maid" , Author = "Nita Prose" },
            new Book { Id = 13, Title = "Olga Dies Dreaming" , Author = "Xochitl Gonzalez" },
            new Book { Id = 14, Title = "Violeta" , Author = "Isabel Allende" },
            new Book { Id = 15, Title = "To Paradise" , Author = "Hanya Yanagihara" },
        };

        IList<Book> classics = new List<Book>
        {
            new Book { Id = 21, Title = "To Kill a Mockingbird", Author = "Harper Lee" },
            new Book { Id = 22, Title = "Little Women", Author = "Louisa May Alcott" },
            new Book { Id = 23, Title = "Beloved", Author = "Toni Morrison" },
        };

        IList<Book> computes = new List<Book>
        {
            new Book { Id = 31, Title = "Windows 11 For Dummies", Author = "Andy Rathbone" },
            new Book { Id = 32, Title = "Code Complete", Author = "Steve McConnell"},
            new Book { Id = 32, Title = "Learning Python, 5th Edition", Author = "Mark Lutz"},
        };

        _sheves = new List<Shelf>
        {
            new Shelf
            {
                Id = 1,
                Theme = "Fiction",
            },

            new Shelf
            {
                Id = 2,
                Theme = "Classics",
            },

            new Shelf
            {
                Id = 3,
                Theme = "Computer",
            }
        };

        foreach (var book in fictions)
        {
            _sheves[0].Books.Add(book);
        }

        foreach (var book in classics)
        {
            _sheves[1].Books.Add(book);
        }

        foreach (var book in computes)
        {
            _sheves[2].Books.Add(book);
        }
    }
    #endregion

    public IEnumerable<Shelf> GetShelves() => _sheves;

    public Shelf CreateShelf(Shelf shelf)
    {
        long maxId = _sheves.Max(s => s.Id);
        shelf.Id = maxId + 1;
        _sheves.Add(shelf);
        return shelf;
    }

    public Shelf GetShelf(long shelfId)
    {
        return _sheves.FirstOrDefault(s => s.Id == shelfId);
    }

    public void DeleteShelf(long shelfId)
    {
        Shelf shelf = _sheves.FirstOrDefault(s => s.Id == shelfId);
        if (shelf is not null)
        {
            _sheves.Remove(shelf);
        }
    }

    public IEnumerable<Book> GetBooks(long shelfId)
    {
        Shelf shelf = GetShelf(shelfId);
        if (shelf is null)
        {
            return Enumerable.Empty<Book>();
        }

        return shelf.Books;
    }

    public Book CreateBook(long shelfId, Book book)
    {
        Shelf shelf = GetShelf(shelfId);
        if (shelf is null)
        {
            throw new InvalidOperationException($"No shelf with Id {shelfId} existed.");
        }

        long maxId = 0;
        if (shelf.Books.Count > 0)
        {
            maxId = shelf.Books.Max(b => b.Id);
        }

        book.Id = maxId + 1;
        shelf.Books.Add(book);
        return book;
    }

    public Book GetBook(long shelfId, long bookId)
    {
        Shelf shelf = GetShelf(shelfId);
        if (shelf is null)
        {
            throw new InvalidOperationException($"No shelf with Id {shelfId} existed.");
        }

        return shelf.Books.FirstOrDefault(book => book.Id == bookId);
    }

    public void DeleteBook(long shelfId, long bookId)
    {
        Shelf shelf = GetShelf(shelfId);
        if (shelf is null)
        {
            throw new InvalidOperationException($"No shelf with Id {shelfId} existed.");
        }

        Book book = shelf.Books.FirstOrDefault(book => book.Id == bookId);
        if (book is not null)
        {
            shelf.Books.Remove(book);
        }
    }
}
