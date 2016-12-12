//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Controllers
{
    using System.Web.Mvc;

    using DataAccessLayer;
    using Models;
    using ViewModels;
    using System.Net.Mail;
    using System.Text;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;


    public class HomeController : Controller
    {
        private DataAccessLayer.SGDbContext db = new DataAccessLayer.SGDbContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(HomePageViewModel model)
        {
            if (ModelState.IsValid)
            {
                // They're going ahead with the order, save their quote for reference?
                /*Quote newQuote = new Quote
                {
                    Cost = model.HomePageQuoteViewModel.Cost,
                    IsMetro = model.HomePageQuoteViewModel.IsMetro,
                    Quantity = model.HomePageQuoteViewModel.Quantity
                };
                db.Quotes.Add(newQuote);
                db.SaveChanges();*///Removed for now
                HttpContext.Session["homePageModel"] = model.HomePageQuoteViewModel;
                return RedirectToAction("Create", "Orders", new { prepopulated = true });
            }

            return View();
        }

        /* Old quote details view controller
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

                // They're going ahead with the order, save their quote so it's locked in for x months, and email it to them...gaining their email address.
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
        */

        public ActionResult About()
        {
            ViewBag.Message = "A little of our story.";

            return View();
        }

      
        public ActionResult Contact()
        {
            ViewBag.Message = "Feel like a chat? Feel free to give us a bell. I'll put the kettle on!";
 
                return View();
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Submit(ContactModels model)
        {
            bool isMessageSent = true;

            if (ModelState.IsValid)
            {
                try
                {
                    await EmailService.SendContactForm(model);
                }
                catch (Exception ex)
                {
                    isMessageSent = false;

                }
            }
            else
            {
                isMessageSent = false;
            }
            return PartialView("_SubmitMessage", isMessageSent);
        }
    }


}
