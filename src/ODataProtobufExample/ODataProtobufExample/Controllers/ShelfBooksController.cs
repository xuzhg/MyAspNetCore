//-----------------------------------------------------------------------------
// <copyright file="ShelfBooksController.cs">
//      Copyright (C) saxu@microsoft.com
// </copyright>
//------------------------------------------------------------------------------

using Bookstores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using ODataProtobufExample.Models;

namespace ODataCustomizePayloadFormat.Controllers;

[Route("odata")]
public class ShelfBooksController : ODataController
{
    private readonly IShelfBookRepository _shelfBookRepository;

    public ShelfBooksController(IShelfBookRepository shelfBookRepository)
    {
        _shelfBookRepository = shelfBookRepository;
    }

    #region Shelves Operation
    [HttpGet("Shelves")]
    [EnableQuery]
    public IActionResult ListShelves()
    {
        return Ok(_shelfBookRepository.GetShelves());
    }

    [HttpGet("Shelves/{shelfId}")]
    [EnableQuery]
    public IActionResult GetShelf(long shelfId)
    {
        Shelf shelf = _shelfBookRepository.GetShelf(shelfId);
        return Ok(shelf);
    }

    [HttpPost("Shelves")]
    [EnableQuery]
    public IActionResult CreateShelf([FromBody] Shelf shelf)
    {
        shelf = _shelfBookRepository.CreateShelf(shelf);
        return Created(shelf);
    }

    [HttpDelete("Shelves/{shelfId}")]
    public IActionResult DeleteShelf(long shelfId)
    {
        _shelfBookRepository.DeleteShelf(shelfId);
        return NoContent();
    }

    #endregion

    #region Books Operations
    [HttpGet("Shelves/{shelf}/Books")]
    [EnableQuery]
    public IActionResult ListBooks(long shelf)
    {
        return Ok(_shelfBookRepository.GetBooks(shelf));
    }

    [HttpGet("Shelves/{shelfId}/Books/{bookId}")]
    [EnableQuery]
    public IActionResult GetBook(long shelfId, long bookId)
    {
        Book book = _shelfBookRepository.GetBook(shelfId, bookId);
        return Ok(book);
    }

    [HttpPost("Shelves/{shelfId}/Books")]
    [EnableQuery]
    public IActionResult CreateBook(long shelfId, [FromBody] Book book)
    {
        book = _shelfBookRepository.CreateBook(shelfId, book);
        return Created(book);
    }

    [HttpDelete("Shelves/{shelfId}/Books/{bookId}")]
    public IActionResult DeleteBook(long shelfId, long bookId)
    {
        _shelfBookRepository.DeleteBook(shelfId, bookId);
        return NoContent();
    }
    #endregion
}

