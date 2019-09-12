﻿using Dapper;
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

        public BookRepository(string connectionString = "")
        {
            ConnectionString = connectionString;
            // ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString
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

        public void AddCopiesOfBook(Book book, int numberOfCopies)
        {
            var sqlString = @"INSERT INTO [BookCopies] VALUES (@ISBN, 0)";

            using (System.Data.IDbConnection db = new SqlConnection(ConnectionString))
            {
                db.Execute(sqlString, Enumerable.Repeat(new { book.ISBN }, numberOfCopies));
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

        public Loan GetLoan(int barcode)
        {
            string sqlString = "SELECT * FROM [Loans] WHERE [Barcode] = @barcode AND [Completed] = 0"; ;
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                return ((List<Loan>)db.Query<Loan>(sqlString, new { barcode })).FirstOrDefault();
            }
        }

        public List<int> GetLastBarcodes(int number)
        {
            string sqlString = "SELECT TOP (@number) * FROM [BookCopies] ORDER BY [Barcode] DESC";
            using (System.Data.IDbConnection db =
                new SqlConnection(ConnectionString))
            {
                List<BookCopy> lastBookCopies = (List<BookCopy>) db.Query<BookCopy>(sqlString, new {number});
                return lastBookCopies.Select(copy => copy.Barcode).ToList();
            }
        }
    }
}
