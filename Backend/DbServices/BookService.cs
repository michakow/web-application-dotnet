using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Backend.DbServices
{
    public class BookService : IBookService
    {
        public readonly AppDbContext db;

        public BookService(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<BookModel> AddBook(BookModel book)
        {
            await db.Books.AddAsync(book);
            await db.SaveChangesAsync();
            return book;
        }

        public async Task<BookModel> DeleteBook(Guid guid)
        {
            var book = await db.Books.FindAsync(guid);
            if (book != null)
            {
                db.Books.Remove(book);
                await db.SaveChangesAsync();
            }
            return book;
        }

        public async Task<BookModel> EditBook(BookModel book)
        {
            var dbBook = await db.Books.FindAsync(book.BookId);
            if (dbBook != null)
            {
                dbBook.Author = book.Author;
                dbBook.Description = book.Description;
                dbBook.Title = book.Title;
                dbBook.Series = book.Series;
                dbBook.Published = book.Published;
                dbBook.Genres = book.Genres;

                await db.SaveChangesAsync();
            }
            
            return dbBook;
        }

        public async Task<BookModel> GetBook(Guid guid)
        {
            return await db.Books.FindAsync(guid);
        }

        public async Task<List<BookModel>> GetBooks()
        {
            return await db.Books.ToListAsync();
        }
    }
}
