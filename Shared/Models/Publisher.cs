using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Model
{
    public class Publisher : ModelBase
    {
        public int PublisherID { get; set; }
        [Column("Publisher Name")]
        public string PublisherName { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
