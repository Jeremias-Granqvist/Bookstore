using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Model
{
    public class BookAuthor : ModelBase
    {
        public long ISBN13 { get; set; }
        public int AuthorID { get; set; }

        public Book Book { get; set; }
        public Author Author { get; set; }
    }
}
