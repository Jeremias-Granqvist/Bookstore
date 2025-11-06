using Data.Context;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository
{
    public class StoreRepository : IStoreRepository
    {
        private readonly BookstoreContext DBcontext;
        public StoreRepository(BookstoreContext context)
        {
            DBcontext = context;
        }
        public async Task<List<Store>> GetStoresAsync()
        {
            return await DBcontext.Stores.ToListAsync();
        }
    }
}
