using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bookish.DataAccess;

namespace Bookish.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var bookRepository = new BookRepository(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

                List<Loan> loans = bookRepository.GetUserLoans(User.Identity.Name);
                Dictionary<Loan, Book> data = new Dictionary<Loan, Book>();

                foreach (var loan in loans)
                {
                    data[loan] = bookRepository.GetBookFromBarcode(loan.Barcode);
                }

                return View(data);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}