using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Model
{
    public class Inventory : ModelBase
    {
		private long _inventoyISBN13;
        [Column("ISBN13")]

        public long InventoryISBN13
		{
			get { return _inventoyISBN13; }
			set 
			{ 
				_inventoyISBN13 = value;
				RaisePropertyChanged();
			}
		}

		private int _storeID;
		public int StoreID
		{
			get { return _storeID; }
			set 
			{ 
				_storeID = value;
				RaisePropertyChanged();
			}
		}

		private int _amount;
		public int Amount
		{
			get { return _amount; }
			set 
			{ 
				_amount = value;
				RaisePropertyChanged();
			}
		}

		public Book book { get; set; }
		public Store store { get; set; }

	}
}
