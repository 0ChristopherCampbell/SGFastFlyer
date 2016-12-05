using SGFastFlyers.DataAccessLayer;
using SGFastFlyers.Models;
using SGFastFlyers.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SGFastFlyers.Controllers
{
    public class HomeController : Controller
    {
        private OrderContext db = new OrderContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(HomePageViewModel model)
        {
            if (ModelState.IsValid)
            {
                HttpContext.Session["homePageModel"] = model.instantQuoteViewModel;
                return RedirectToAction("QuoteDetail");
            }

            return View();
        }


        public ActionResult QuoteDetail()
        {
            if (HttpContext.Session["homePageModel"] != null)
            {
                InstantQuoteViewModel model = (InstantQuoteViewModel)HttpContext.Session["homePageModel"];
                return View(model);
            }

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult QuoteDetail(InstantQuoteViewModel emptyModel)
        {
            if (HttpContext.Session["homePageModel"] != null)
            {
                InstantQuoteViewModel model = (InstantQuoteViewModel)HttpContext.Session["homePageModel"];

                Quote newQuote = new Quote
                {
                    Cost = model.Cost,
                    IsMetro = model.IsMetro,
                    Quantity = model.Quantity
                };

                // They're going ahead with the order, save their quote so it's locked in for x months
                db.Quotes.Add(newQuote);
                db.SaveChanges();

                CreateOrderViewModel newOrder = new CreateOrderViewModel
                {
                    Quantity = model.Quantity,
                    IsMetro = model.IsMetro,
                    NeedsPrint = model.NeedsPrint,
                    PrintSize = model.PrintSize,
                    PrintFormat = model.PrintFormat,
                    Cost = model.Cost
                };

                HttpContext.Session["instantQuoteOrder"] = newOrder;
                return RedirectToAction("Create", "Orders", new { prepopulated = true });
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