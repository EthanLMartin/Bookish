using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataAccess
{
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public Book(string isbn, string title, string author)
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
