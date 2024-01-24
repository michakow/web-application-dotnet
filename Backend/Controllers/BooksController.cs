using Backend.DbServices;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<BookModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            var books = await bookService.GetBooks();
            return StatusCode(StatusCodes.Status200OK, books);
        }

        [HttpGet("{guid}")]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(Guid guid)
        {
            var book = await bookService.GetBook(guid);
            Response<BookModel> response = new Response<BookModel> { Message = "", Data = book };

            if (book == null)
            {
                response.Message = $"Nie znaleziono książki o guid {guid}";
                return StatusCode(StatusCodes.Status404NotFound, response);
            }

            response.Message = $"Pobrano książkę o guid {guid}";
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookModel>> AddBook(BookModel book)
        {
            Response<BookModel> response = new Response<BookModel> { Message = "", Data = book };

            if (checkIfRequestBodyHasNull(book))
            {
                response.Message = "Żądanie niepoprawne";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            var dbBook = await bookService.AddBook(book);
            response.Data = dbBook;

            if (dbBook == null)
            {
                response.Message = "Nie udało się dodać książki do kolekcji";
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            response.Message = "Dodano nową książkę do kolekcji";
            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpDelete("{guid}")]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(Guid guid)
        {
            var book = await bookService.DeleteBook(guid);
            Response<BookModel> response = new Response<BookModel> { Message = "", Data = book };

            if (book == null)
            {
                response.Message = $"Nie znaleziono książki o guid {guid}";
                return StatusCode(StatusCodes.Status404NotFound, response);
            }

            response.Message = $"Usunięto książkę o guid {guid}";
            return StatusCode(StatusCodes.Status200OK, response);
        
        }

        [HttpPut("{guid}")]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<BookModel>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditBook(Guid guid, BookModel book)
        {
            Response<BookModel> response = new Response<BookModel> { Message = "", Data = book };

            if (checkIfRequestBodyHasNull(book) || guid != book.BookId)
            {
                response.Message = "Żądanie niepoprawne";
                return StatusCode(StatusCodes.Status400BadRequest, response);
            }

            var dbBook = await bookService.EditBook(book);
            response.Data = dbBook;

            if (dbBook == null)
            {
                response.Message = $"Nie znaleziono książki o guid {guid}";
                return StatusCode(StatusCodes.Status404NotFound, response);
            }

            response.Message = $"Zmodyfikowano książkę o guid {guid}";
            return StatusCode(StatusCodes.Status200OK, response);

        }

        private Boolean checkIfRequestBodyHasNull(BookModel book)
        {
            return book.Author == null || book.Description == null || book.Title == null || book.Series == null || book.Published == null || book.Genres == null;
        }
    }
}
