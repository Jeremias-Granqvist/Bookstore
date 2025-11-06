using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Statics;
using Data.Interfaces;

namespace Data.Repository
{
    public class BookRepository : ModelBase, IBookRepository
    {
        private readonly BookstoreContext DBcontext;

        public BookRepository(BookstoreContext context)
        {
            DBcontext = context;
        }
        public async Task<List<Book>> GetBooksAsync()
        {
            using (DBcontext) 
                return await DBcontext.Books
                    .Include(b => b.Authors)
                    .ToListAsync();
        }
        public async Task<bool> AddBookAsync(Book book)
        {
            using (DBcontext)
            {
                DBcontext.Books.Add(book);
                foreach (var author in book.Authors)
                {
                    DBcontext.BookAuthors.Add(new BookAuthor
                    {
                        ISBN13 = book.ISBN13,
                        AuthorID = author.AuthorID
                    });
                }
                var response = await DBcontext.SaveChangesAsync();
                return StaticMethods.IsCrudOperationSuccessful(response);
            }
        }
        public async Task<bool> EditBookAsync(Book book)
        {
            using (DBcontext)
            {

                var existingBook = DBcontext.Books.Include(b => b.Authors)
                    .FirstOrDefault(b => b.ISBN13 == book.ISBN13);

                if (existingBook != null)
                {
                    existingBook.ISBN13 = book.ISBN13;
                    existingBook.Title = book.Title;
                    existingBook.ReleaseDate = book.ReleaseDate;
                    existingBook.Price = book.Price;
                    existingBook.Language = book.Language;

                    existingBook.Authors.Clear();
                    foreach (var author in book.Authors)
                    {
                        var existingAuthor = DBcontext.Authors.Local.FirstOrDefault(a => a.AuthorID == author.AuthorID);
                        if (existingAuthor == null)
                        {
                            DBcontext.Authors.Attach(author);
                            existingBook.Authors.Add(author);
                            DBcontext.BookAuthors.Add(new BookAuthor
                            {
                                ISBN13 = existingBook.ISBN13,
                                AuthorID = author.AuthorID
                            });
                        }
                        else
                        {
                            existingBook.Authors.Add(existingAuthor);
                        }
                    }
                    var response = await DBcontext.SaveChangesAsync();
                    return StaticMethods.IsCrudOperationSuccessful(response);
                }
                else
                {
                    existingBook = DBcontext.Books.Include(b => b.Authors)
                        .FirstOrDefault(b => b.ISBN13 == existingBook.ISBN13);

                    existingBook.ISBN13 = book.ISBN13;
                    existingBook.Title = book.Title;
                    existingBook.ReleaseDate = book.ReleaseDate;
                    existingBook.Price = book.Price;
                    existingBook.Language = book.Language;
                    existingBook.Authors.Clear();

                    foreach (var author in book.Authors)
                    {
                        var existingAuthor = DBcontext.Authors.Local.FirstOrDefault(a => a.AuthorID == author.AuthorID);
                        if (existingAuthor == null)
                        {
                            DBcontext.Authors.Attach(author);
                            existingBook.Authors.Add(author);
                        }
                        else
                        {
                            existingBook.Authors.Add(existingAuthor);
                        }
                    }
                    var response = await DBcontext.SaveChangesAsync();
                    return StaticMethods.IsCrudOperationSuccessful(response);
                }
            }
        }
        public async Task<bool> DeleteBookAsync(long ISBN)
        {
            using (DBcontext)
            {
                var bookToDelete = DBcontext.Books.FirstOrDefault(b => b.ISBN13 == ISBN);
                DBcontext.Remove(bookToDelete);
                var response = await DBcontext.SaveChangesAsync();
                return StaticMethods.IsCrudOperationSuccessful(response);
            }
        }
    }
}
