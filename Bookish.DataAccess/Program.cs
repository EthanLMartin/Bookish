using Bookish.DataAccess;
using Dapper;
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
            IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            string SqlString = "SELECT TOP 100 [ISBN],[title],[author] FROM [Books]";

            var books = (List<Book>)db.Query<Book>(SqlString);

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