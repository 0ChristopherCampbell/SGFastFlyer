﻿//-----------------------------------------------------------------------
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
    using Utility;
    using System.IO;

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
            if (!string.IsNullOrEmpty(Request.Form["hdnorderNow"]) && ModelState.IsValid)
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
            if (!string.IsNullOrEmpty(Request.Form["hdnemailQuote"]) && ModelState.IsValid)
            {
                EmailQuotes model1 = new EmailQuotes();
                HttpContext.Session["homePageModel1"] = model.HomePageQuoteViewModel;
                return View("EmailQuote", model1);
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EmailQuote(EmailQuotes model1)
        {

            {
                InstantQuoteViewModel model = (InstantQuoteViewModel)HttpContext.Session["homePageModel1"];



                if (ModelState.IsValid && (model.NeedsPrint == true))
                {
                    var url = @"\home\index";
                    var linkText = "Click here";
                    var body = "Hi {6}, </br><p>Here is your quote: </p></br><p>Quantity: {0}</p><p>Metro Area: {1}</p><p>Is printing required: {2}" +
                        "</p><p>Print Size: {3}</p><p>Double Sided: {4}</p><p>Price: {5}</p></br><p>Thank you for your interest. Please reply to this email to place an order.</p><p>Kind Regards,</p>SG Fast Flyers.";
                    string attach = Server.MapPath(@"\Content\Documents\SGFastFlyers_Letterbox_Printing_&_Delivery_Details.pdf");
                    string href = String.Format("<a href='{0}'>{1}</a>", url, linkText);
                    string yourEncodedHtml = "Quote Sent Successfully.<br/>" + href + " to send another one if you like.<br/><p>Have a great day.<p/>";
                    var html = new MvcHtmlString(yourEncodedHtml);
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model1.Email));  // replace with valid value
                    message.Bcc.Add(new MailAddress(Config.sgEmail));
                    message.From = new MailAddress(Config.sgEmail);  // replace with valid value
                    message.Subject = "Your Quote";
                    message.Body = string.Format(body, model.Quantity, (model.IsMetro == true ? Config.Yes : Config.No), (model.NeedsPrint == true ? Config.Yes : Config.No), model.PrintSize, 
                        (model.IsDoubleSided == true ? Config.Yes : Config.No), model.FormattedCost, model1.FirstName);
                    message.IsBodyHtml = true;
                    message.Attachments.Add(new Attachment(attach));



                    try
                    {
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory;
                            await smtp.SendMailAsync(message);

                        }
                        ViewBag.Status = html;
                        ModelState.Clear();
                    }
                    catch (Exception)
                    {
                        ViewBag.Status = "Problem while sending email, Please check details.";

                    }
                }
                if (ModelState.IsValid && (model.NeedsPrint == false))
                {
                    var url = @"\home\index";
                    var linkText = "Click here";
                    var body = "Hi {0}, </br><p>Here is your quote: </p></br><p>Quantity: {1}</p><p>Metro Area: {2}</p>" +
                        "<p>Price: {3}</p></br><p>Thank you for your interest. Please reply to this email to place an order.</p><p>Kind Regards,</p>SG Fast Flyers.";
                    string attach = Server.MapPath(@"\Content\Documents\SGFastFlyers_Letterbox_Printing_&_Delivery_Details.pdf");
                    string href = String.Format("<a href='{0}'>{1}</a>", url, linkText);
                    string yourEncodedHtml = "Quote Sent Successfully.<br/>" + href + " to send another one if you like.<br/><p>Have a great day.<p/>";
                    var html = new MvcHtmlString(yourEncodedHtml);
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model1.Email));  // replace with valid value
                    message.Bcc.Add(new MailAddress(Config.sgEmail));
                    message.From = new MailAddress(Config.sgEmail);  // replace with valid value
                    message.Subject = "Your Quote";
                    message.Body = string.Format(body, model1.FirstName, model.Quantity, (model.IsMetro == true ? Config.Yes : Config.No), 
                        model.FormattedCost);
                    message.IsBodyHtml = true;
                    message.Attachments.Add(new Attachment(attach));


                    try
                    {
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            await smtp.SendMailAsync(message);

                        }
                        ViewBag.Status = html;
                        ModelState.Clear();
                    }
                    catch (Exception)
                    {
                        ViewBag.Status = "Problem while sending email, Please check details.";

                    }
                }
                return View();
            }
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
                var body = "<p>Email From: {0} {1} ({2}) Phone: {4}<p>Message:</p><p>{3}</p>";
                string yourEncodedHtml = "Email Sent Successfully.<br/>Feel free to send another one if you like.<br/><p>Have a great day.<p/>";
                var html = new MvcHtmlString(yourEncodedHtml);
                var message = new MailMessage();
                message.To.Add(new MailAddress(Config.sgEmail));  // replace with valid value 
                message.From = new MailAddress(model.Email);  // replace with valid value
                message.Subject = model.Subject;
                message.Body = string.Format(body, model.FirstName, model.LastName, model.Email, model.Comment, model.PhoneNumber);
                message.IsBodyHtml = true;
                if (Attachment != null)
                {
                    string pathToSave = Server.MapPath(Config.objectDataPath) + "\\Temp\\" + model.Email.Replace("@", "_").Replace(".", "_").ToString() + "\\";
                    IO.CreateFolder(pathToSave);
                    IO.DeleteFilesInFolder(pathToSave);
                    Attachment.SaveAs(pathToSave + Path.GetFileName(Attachment.FileName));

                    var attachment = new Attachment(pathToSave + Path.GetFileName(Attachment.FileName));
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
                catch (Exception)
                {
                    ViewBag.Status = "Problem while sending email, Please check details.";

                }
            }
            return View();
        }


    }
}




