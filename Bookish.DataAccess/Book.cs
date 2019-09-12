using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataAccess
{
    public class Book
    {
        public string ISBN { get; set; } = null;
        public string Title { get; set; } = null;
        public string Author { get; set; } = null;

        public Book(string isbn=null, string title=null, string author=null)
        {
            ISBN = isbn;
            Title = title;
            Author = author;
        }

        public bool IsValidBook()
        {
            if (String.IsNullOrEmpty(ISBN) || String.IsNullOrEmpty(Title) || String.IsNullOrEmpty(Author))
                return false;
            return true;
        }
    }
}
