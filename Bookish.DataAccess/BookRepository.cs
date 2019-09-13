using Dapper;
using System;
using System.Collections;
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

        public BookRepository(string connectionString)
        {
            ConnectionString = connectionString ?? ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public void AddBook(Book book)
        {
            var sqlString = "INSERT INTO [Books] VALUES (@ISBN, @Title, @Author)";
            if (!book.IsValidBook() || GetBookFromISBN(book.ISBN) != null)
                return;
            using (System.Data.IDbConnection db = new SqlConnection(ConnectionString))
            {
                db.Execute(sqlString, new {book.ISBN, book.Title, book.Author});
            }
        }

        public List<int> AddCopiesOfBook(Book book, int numberOfCopies)
        {
            const string insertSqlString = @"INSERT INTO [BookCopies] VALUES (@ISBN, 0)";
            const string selectSlString = "SELECT TOP (@numberOfCopies) * FROM [BookCopies] ORDER BY [Barcode] DESC";

            using (System.Data.IDbConnection db = new SqlConnection(ConnectionString))
            {
                    db.Execute(insertSqlString, Enumerable.Repeat(new {book.ISBN}, numberOfCopies));
                    var lastBookCopies = db.Query<BookCopy>(selectSlString, new {numberOfCopies});
                    return lastBookCopies.Select(copy => copy.Barcode).ToList();
            }
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

        public List<Book> FindBooks(string search)
        {
            string searchTerm = "%" + search + "%";
            string SqlString = "SELECT * FROM [Books]" +
                               " WHERE [Title] LIKE @searchTerm" +
                               " OR [Author] LIKE @searchTerm";

            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return (List<Book>)db.Query<Book>(SqlString, new { searchTerm });
            }
        }

        public Book GetBookFromISBN(String ISBN)
        {
            string sqlString = "SELECT * FROM [Books] WHERE [ISBN] = @ISBN";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return ((List<Book>)db.Query<Book>(sqlString, new { ISBN })).FirstOrDefault();
            }
        }
        public Book GetBookFromBarcode(int barcode)
        {
            string sqlString = "SELECT b.* " +
                               "FROM [Books] b JOIN [BookCopies] c ON b.ISBN = c.ISBN " +
                               "WHERE c.Barcode = @barcode";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return ((List<Book>)db.Query<Book>(sqlString, new {barcode} )).FirstOrDefault();
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

        public BookCopy GetAvailableBookCopy(string ISBN)
        {
            string sqlString = "SELECT TOP (1) * FROM [BookCopies] WHERE [ISBN] = @ISBN AND [Borrowed] = 0";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return db.Query<BookCopy>(sqlString, new { ISBN }).FirstOrDefault();
            }
        }

        public bool LoanBook(string userName, string ISBN)
        {
            var bookCopy = GetAvailableBookCopy(ISBN);
            if (bookCopy == null)
                return false;

            UpdateBookCopyBorrowedField(bookCopy.Barcode, true);
            AddLoan(bookCopy.Barcode, userName);
            return true;
        }

        private void UpdateBookCopyBorrowedField(int barcode, bool borrowed)
        {
            var sqlString = "UPDATE [BookCopies] SET [Borrowed] = @borrowed WHERE [Barcode] = @Barcode";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                db.Execute(sqlString, new {barcode, borrowed});
            }
        }

        private void AddLoan(int barcode, string userName)
        {
            var dueDate = DateTime.Now.AddDays(14);
            var sqlString = "INSERT INTO [Loans] VALUES(@barcode, @userName, @dueDate, 0)";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                db.Execute(sqlString, new { barcode, userName, dueDate });
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

        public Loan GetLoan(int barcode)
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
