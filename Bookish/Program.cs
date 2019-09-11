using Bookish.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            var bookManager = new BookRepository();

            var books = bookManager.GetAllBooks();

            foreach (var book in books)
            {
                Console.WriteLine(new string('*', 20));
                Console.WriteLine("\nBook ISBN: " + book.ISBN);
                Console.WriteLine("Book Title: " + book.Title);
                Console.WriteLine("Author: " + book.Author + "\n");
                Console.WriteLine(new string('*', 20));
            }

            Console.ReadLine();
        }
    }
}