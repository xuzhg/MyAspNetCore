//-----------------------------------------------------------------------------
// <copyright file="BooksController.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ODataCborExample.Models;

namespace ODataCborExample.Controllers;

public class BooksController : ControllerBase
{
    private static IList<Book> _books = GetBooks();

    [HttpGet]
    [EnableQuery]
    public IActionResult Get()
    {
        return Ok(_books);
    }

    [HttpGet]
    [EnableQuery]
    public Book Get(int key)
    {
        Book b = _books.FirstOrDefault(c => c.Id == key);
        return b;
    }

    public Book Post([FromBody]Book book)
    {
        return book;
    }

    private static IList<Book> GetBooks()
    => new List<Book>
    {
        new Book { Id = 1, Title = "1984", Author = "George Orwell", ISBN = "978-0-451-52493-5", Pages = 268 },
        new Book { Id = 2, Title = "Animal Farm", Author = "George Orwell", ISBN = "978-0-451-52434-2", Pages = 144 },
        new Book { Id = 3, Title = "Brave New World", Author = "Aldous Huxley", ISBN = "978-0-060-92987-9", Pages = 288 },
        new Book { Id = 4, Title = "Fahrenheit 451", Author = "Ray Bradbury", ISBN = "978-0-345-34296-6", Pages = 208 },
        new Book { Id = 5, Title = "Essential C#5.0", Author = "Mark Michaelis", ISBN = "978-0-321-87758-1", Pages = 388 },
        new Book { Id = 6, Title = "Enterprise Games", Author = "Michael Hugos", ISBN = "063-6-920-02371-5", Pages = 788 }
    };
}
