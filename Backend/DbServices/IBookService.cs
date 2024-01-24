using Backend.Models;

namespace Backend.DbServices
{
    public interface IBookService
    {
        Task<BookModel> AddBook(BookModel book);

        Task<BookModel> EditBook(BookModel book);

        Task<BookModel> DeleteBook(Guid guid);

        Task<List<BookModel>> GetBooks();

        Task<BookModel> GetBook(Guid guid);
    }
}
