using Data.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Managers
{
    public class StoreManager : IStoreManager
    {
        private readonly BookstoreContext DBcontext;
        public StoreManager(BookstoreContext context)
        {
            DBcontext = context;
        }
        public async Task<List<Store>> GetStoresAsync()
        {
            return await DBcontext.Stores.ToListAsync();
        }
    }
}
