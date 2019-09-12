using Bookish.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            var bookRepositry = new BookRepository();


            return View(bookRepositry.GetAllBooks().OrderBy(book => book.Title).ToList());
        }

        [HttpGet]
        public ActionResult Copies(String ISBN)
        {
            var bookRespository = new BookRepository();
            ViewData["BookTitle"] = bookRespository.GetBook(ISBN).Title;
            return View(bookRespository.GetBookCopies(ISBN));
        }
    }
}