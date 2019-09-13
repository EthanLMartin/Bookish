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
        public ActionResult Catalogue(string search = null)
        {
            var bookRepository = new BookRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            var books = new List<Book>();

            if (search != null)
            {
                books = bookRepository.FindAllBooks(search).OrderBy(book => book.Title).ToList();
                TempData["Search"] = true;
            }
            else
            {
                books = bookRepository.GetAllBooks().OrderBy(book => book.Title).ToList();
            }

            return View(books);
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

            TempData["Barcodes"] = bookRepository.GetLastBarcodes(model.NumberOfCopies);

            return Redirect("Success");
        }

        [HttpGet]
        public ActionResult Success()
        {
            if (TempData["Barcodes"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(TempData["Barcodes"]);
        }

        public ActionResult LoanBook(string ISBN)
        {
            var bookRepository = new BookRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            TempData["Borrowed"] = false;
            if (bookRepository.GetAvailableBookCopy(ISBN) != null && User.Identity.IsAuthenticated)
            {
                bookRepository.LoanBook(User.Identity.Name, ISBN);
                TempData["Borrowed"] = true;
            }

            return Redirect("Copies/?ISBN=" + ISBN);
        }
    }
}