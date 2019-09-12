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
        private readonly string ConnectionString;

        public BookRepository(string connectionString = "")
        {
            ConnectionString = connectionString;
            // ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
        }
        public List<Book> GetAllBooks()
        {
            string SqlString = "SELECT * FROM [Books]";

            using (System.Data.IDbConnection db = 
                new SqlConnection(ConnectionString))
            {
                return (List<Book>)db.Query<Book>(SqlString);
            }
        }

        public Book GetBook(String ISBN)
        {
            string sqlString = "SELECT * FROM [Books] WHERE [ISBN] = @ISBN";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return ((List<Book>)db.Query<Book>(sqlString, new { ISBN })).FirstOrDefault();
            }
        }
        public Book GetBookFromBarcode(String barcode)
        {
            string sqlString = "SELECT b.* " +
                               "FROM [Books] b JOIN [BookCopies] c ON b.ISBN = c.ISBN";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return ((List<Book>)db.Query<Book>(sqlString)).FirstOrDefault();
            }
        }

        public List<Loan> GetUserLoans(String userName)
        {
            string sqlString = "SELECT * FROM [Loans] WHERE" +
                               " [UserId] = @userName AND [Completed] = 0";

            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return (List<Loan>)db.Query<Loan>(sqlString, new {userName});
            }
        }

        public List<BookCopy> GetBookCopies(String ISBN)
        {
            string sqlString = "SELECT * FROM [BookCopies] WHERE [ISBN] = @ISBN";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                var bookCopies = (List<BookCopy>) db.Query<BookCopy>(sqlString, new { ISBN });

                return bookCopies;
            }
        }

        public Loan GetLoan(string barcode)
        {
            string sqlString = "SELECT * FROM [Loans] WHERE [Barcode] = @barcode AND [Completed] = 0"; ;
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return ((List<Loan>)db.Query<Loan>(sqlString, new { barcode })).FirstOrDefault();
            }
        }
    }
}
