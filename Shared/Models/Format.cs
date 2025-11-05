using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Model
{
    public class Format : ModelBase
    {
        public int FormatID { get; }
        public string FormatTyp { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();


    }
}
