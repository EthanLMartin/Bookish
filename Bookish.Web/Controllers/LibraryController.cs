using Bookish.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookish.Web.Models;

namespace Bookish.Web.Controllers
{
    public class LibraryController : Controller
    {
        // GET: Library
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Catalogue()
        {
            var bookRepository = new BookRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            return View(bookRepository.GetAllBooks().OrderBy(book => book.Title).ToList());
        }

        [HttpGet]
        public ActionResult Copies(string ISBN)
        {
            var bookRepository = new BookRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            ViewData["BookTitle"] = bookRepository.GetBookFromISBN(ISBN).Title;

            var bookCopies = bookRepository.GetBookCopies(ISBN);

            foreach (var bookCopy in bookCopies)
            {
                if (bookCopy.Borrowed)
                {
                    bookCopy.Loan = bookRepository.GetLoan(bookCopy.Barcode);
                }
            }
            return View(bookCopies);
        }

        [AllowAnonymous]
        public ActionResult AddBook(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult AddBook(AddBookViewModel model, string returnUrl)
        {
            var bookRepository = new BookRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            var book = new Book(model.ISBN, model.Title, model.Author);
            bookRepository.AddBook(book);
            bookRepository.AddCopiesOfBook(book, model.NumberOfCopies);

            return View();
        }
    }
}