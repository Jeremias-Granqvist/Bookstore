using Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Statics
{
    public static class StaticConstants
    {
        public const string API_BASE_ADRESS = "http://localhost:5252";
        public const string PICK_AUTH_EDIT = "Please choose an author to edit.";
        public const string PICK_BOOK_EDIT = "Please choose a book to edit";
        public const string FILL_ALL_FIELDS = "Please fill out all fields.";
        public const string INVALID_ISBN = "Please ensure your ISBN number is valid";
        public const string ERROR = "Error";
        public const string DELETE_WARNING =
        "This will permanently remove {0} from the database, do you wish to continue?";

    }
}
