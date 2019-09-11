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

            ViewData["Books"] = bookRepositry.GetAllBooks();

            return View(bookRepositry.GetAllBooks());
        }
    }
}