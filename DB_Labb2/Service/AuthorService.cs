using Bookstore.Service.Interfaces;
using Shared.Model;
using Shared.Statics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
namespace Bookstore.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly HttpClient _httpClient;
        private readonly IEventDispatcher _eventDispatcher;
        public AuthorService(IHttpClientFactory httpClient, IEventDispatcher eventDispatcher)
        {
            _httpClient = httpClient.CreateClient("api");
            _httpClient.BaseAddress = new Uri (StaticConstants.API_BASE_ADRESS); 
            _eventDispatcher = eventDispatcher;
        }
        public async Task<List<Author>> GetAuthorsAsync()
        {
            try
            {
            var response = await _httpClient.GetFromJsonAsync<List<Author>>("api/Author/GetAuthors");
                _eventDispatcher.EntityList(response);
            return response ?? new List<Author>();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching authors: {ex.Message}");

                throw;
            }
        }
        public async Task<Author> AddAuthorAsync(Author author)
        {
            try
            {
            var response = await _httpClient.PostAsJsonAsync<Author>("api/Author/CreateAuthor", author);
                if (response.IsSuccessStatusCode) _eventDispatcher.EntityAdded(author);
                return author;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }
        }
        public async Task<Author> DeleteAuthorAsync(Author author)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/Author/DeleteAuthor/{author.AuthorID}");
                if (response.IsSuccessStatusCode) _eventDispatcher.EntityRemoved(author);
                return author;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}, {ex.InnerException}");
                throw;
            }
        }
        public async Task<bool>EditAuthorAsync(Author author)
        {
            try
            {
            var response = await _httpClient.PutAsJsonAsync($"api/Author/UpdateAuthor/{author.AuthorID}", author);
            if (response.IsSuccessStatusCode)  _eventDispatcher.EntityUpdated(author);
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
