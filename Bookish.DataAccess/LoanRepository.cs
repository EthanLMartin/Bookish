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
    public class LoanRepository
    {
        private readonly string ConnectionString;

        public LoanRepository(string connectionString)
        {
            ConnectionString = connectionString ?? ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public List<Loan> GetUserLoans(string userName)
        {
            string sqlString = "SELECT * FROM [Loans] WHERE" +
                               " [UserId] = @userName AND [Completed] = 0";

            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return (List<Loan>)db.Query<Loan>(sqlString, new { userName });
            }
        }

        public bool LoanBook(string userName, string ISBN)
        {
            var bookRepo = new BookRepository(ConnectionString);

            var bookCopy = bookRepo.GetAvailableBookCopy(ISBN);
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
                db.Execute(sqlString, new { barcode, borrowed });
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