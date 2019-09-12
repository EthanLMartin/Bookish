using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookish.DataAccess
{
    public class BookRepository
    {
        public List<Book> GetAllBooks()
        {
            string SqlString = "SELECT * FROM [Books]";

            using (System.Data.IDbConnection db = 
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return (List<Book>)db.Query<Book>(SqlString);
            }
        }

        public Book GetBook(String ISBN)
        {
            string sqlString = "SELECT * FROM [Books] WHERE [ISBN] = " + ISBN;
            using (System.Data.IDbConnection db =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                return ((List<Book>)db.Query<Book>(sqlString)).FirstOrDefault();
            }
        }

        public List<BookCopy> GetBookCopies(String ISBN)
        {
            string sqlString = "SELECT * FROM [BookCopies] WHERE [ISBN] = " + ISBN;
            using (System.Data.IDbConnection db =
                new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                var bookCopies = (List<BookCopy>) db.Query<BookCopy>(sqlString);
                foreach (var bookCopy in bookCopies)
                {
                    if (bookCopy.Borrowed)
                    {
                        sqlString = "SELECT * FROM [Loans] WHERE [Barcode] = " + bookCopy.Barcode + 
                            " AND [Completed] = 0";
                        bookCopy.Loan = ((List<Loan>) db.Query<Loan>(sqlString)).FirstOrDefault();
                    }
                }

                return bookCopies;
            }
        }
    }
}
