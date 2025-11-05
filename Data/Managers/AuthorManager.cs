using Azure;
using Data.Context;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Model;
using Shared.Statics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace Data.Managers
{
    public class AuthorManager : ModelBase, IAuthorManager
    {
        private readonly BookstoreContext DBcontext;
        public AuthorManager(BookstoreContext context)
        {
            DBcontext = context;
        }
        public async Task<bool> AddAuthorAsync(Author author)
        {
            using (DBcontext)
            {
                DBcontext.Add(author);
                var response = await DBcontext.SaveChangesAsync();
                return StaticMethods.IsCrudOperationSuccessful(response);
            }
        }
        public async Task<List<Author>> GetAuthorsAsync()
        {
            using (DBcontext) return await DBcontext.Authors.ToListAsync();
        }
        public async Task<bool> EditAuthorAsync(Author updatedAuthor)
        {
            using (DBcontext)
            {
                var existingAuthor = DBcontext.Authors.FirstOrDefault(a => a.AuthorID == updatedAuthor.AuthorID);
                    existingAuthor.Firstname = updatedAuthor.Firstname;
                    existingAuthor.Lastname = updatedAuthor.Lastname;
                    existingAuthor.Birthdate = updatedAuthor.Birthdate;
                    var response = await DBcontext.SaveChangesAsync();
                return StaticMethods.IsCrudOperationSuccessful(response);
            }
        }
        public async Task<bool> DeleteAuthorAync(int authorID)
        {
            using (DBcontext)
            {
                var authorToDelete = DBcontext.Authors.FirstOrDefault(a => a.AuthorID == authorID);

                    DBcontext.Remove(authorToDelete);
                    var response = await DBcontext.SaveChangesAsync();
                return StaticMethods.IsCrudOperationSuccessful(response);
            }
        }
    }
}
