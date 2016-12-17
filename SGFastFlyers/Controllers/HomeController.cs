//-----------------------------------------------------------------------
// <copyright file="HomeController.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Controllers
{
    using System.Web.Mvc;
    using Models;
    using ViewModels;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using System.Web;
    using System;


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
        public async Task<ActionResult> Contact(ContactModels model, HttpPostedFileBase Attachment)
        {
            if (ModelState.IsValid)
            {
                var body = "<p>Email From: {0} {1} ({2})><p>Message:</p><p>{3}</p>";
                string yourEncodedHtml = "Email Sent Successfully.<br/>Feel free to send another one if you like.<br/><p>Have a great day.<p/>";
                var html = new MvcHtmlString(yourEncodedHtml);
                var message = new MailMessage();
                message.To.Add(new MailAddress("contact_us@sgfastflyers.com.au"));  // replace with valid value 
                message.From = new MailAddress(model.Email);  // replace with valid value
                message.Subject = model.Subject;
                message.Body = string.Format(body, model.FirstName, model.LastName, model.Email, model.Comment);
                message.IsBodyHtml = true;
                if (Attachment != null)
                {
                    var attachment = new Attachment(Request.Files[0].FileName);
                    message.Attachments.Add(attachment);
                }
                try
                {
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        await smtp.SendMailAsync(message);

                    }
                    ViewBag.Status = html;
                    ModelState.Clear();
                }
                catch (Exception )
                {
                    ViewBag.Status = "Problem while sending email, Please check details.";
                   
                }  
            }
            return View(); 
        }
    }
}



