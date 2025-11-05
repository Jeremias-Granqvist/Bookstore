using Bookstore.Service.Interfaces;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Bookstore.Service
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly IEventDispatcher _eventDispatcher;
        public BookService(IHttpClientFactory httpClient, IEventDispatcher eventDispatcher)
        {
            _httpClient = httpClient.CreateClient("api");
            _eventDispatcher = eventDispatcher;
        }
        public async Task<List<Book>> GetBooksAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<Book>>("api/Book/GetBooks");
            if (response is null) return new List<Book>();
            return response;
        }
        public async Task<Book> AddBookAsync(Book book)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<Book>("api/CreateBook", book);
                if (response.IsSuccessStatusCode) _eventDispatcher.EntityAdded(book);
                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }
        }
        public async Task<Book> DeleteBookAsync(Book book)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/DeleteBook/{book.ISBN13}");
                if (response.IsSuccessStatusCode) _eventDispatcher.EntityRemoved(book);
                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }
        }
        public async Task<bool> EditBookAsync(Book book)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/UpdateBook/{book.ISBN13}", book);
                if (response.IsSuccessStatusCode) 
                { 
                    _eventDispatcher.EntityUpdated(book);
                    _eventDispatcher.EntityUpdated(book.Authors);
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }
        }

    }
}

