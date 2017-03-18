//-----------------------------------------------------------------------
// <copyright file="OrdersController.cs" company="SGFastFlyers">
//     Copyright (c) SGFastFlyers. All rights reserved.
// </copyright>
// <author> Christopher Campbell </author>
//-----------------------------------------------------------------------
namespace SGFastFlyers.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;
    using DataAccessLayer;
    using Models;
    using Utility;
    using ViewModels;
    using System.Threading.Tasks;
    using System.Net.Mail;
    using System.IO;
    using System.Web;
    using Stripe;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using Excel;    
    /// <summary>
                    /// The order controller. Order creation and payment is handled here.
                    /// </summary>

    public class OrdersController : Controller
    {
        /// <summary>
        /// The database context
        /// </summary>
        private SGDbContext db = new SGDbContext();
        IExcelDataReader excelReader;       
       
        /// <summary>
        /// GET: Orders
        /// </summary>
        /// <returns>Default (usually List) view of orders</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var orders = this.db.Orders.Include(o => o.DeliveryDetail).Include(o => o.PrintDetail);
            return this.View(orders.ToList());
        }

        /// <summary>
        /// GET: Orders/Details/5
        /// </summary>
        /// <param name="id">the orders id</param>
        /// <returns>The order detail view</returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = this.db.Orders.Find(id);
            if (order == null)
            {
                return this.HttpNotFound();
            }

            return this.View(order);
        }

        /// <summary>
        /// GET: Orders/Create
        /// </summary>
        /// <returns>The create order form</returns>
        public ActionResult Create()
        {
            return this.View();
        }



        /// <summary>
        /// <c>GET: Orders/Create?prepopulated=bool</c>
        /// </summary>
        /// <param name="prepopulated">whether or not the data has been prepopulated on the instant-quote tool</param>
        /// <returns>The create order page</returns>
        [HttpGet]
        [DataType(DataType.Date)]
        public ActionResult Create(bool prepopulated = false)
        {
            if (HttpContext.Session["homePageModel"] != null && prepopulated)
            {
                //DateTime dateTime = DateTime.UtcNow.Date;
                InstantQuoteViewModel model = (InstantQuoteViewModel)HttpContext.Session["homePageModel"];
                CreateOrderViewModel orderModel = new CreateOrderViewModel
                {
                    Cost = model.Cost,
                    NeedsPrint = model.NeedsPrint,
                    IsDoubleSided = model.IsDoubleSided,
                    IsMetro = model.IsMetro,
                    Quantity = model.Quantity,
                    PrintSize = model.PrintSize
                };

                return this.View(orderModel);
            }

            return this.View();
        }

        /// <summary>
        /// POST: Orders/Create
        /// TODO: - Paypal integration
        ///         - Eventual on-site payment
        /// </summary>
        /// <param name="createOrderViewModel">The order in view model form to be bound to models.</param>
        /// <returns>A redirect to payment and eventual payment complete page.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,FirstName,LastName,EmailAddress,PhoneNumber,Quantity,DeliveryDate,IsMetro,DeliveryArea,NeedsPrint,PrintSize,PrintFormat,IsDoubleSided,Attachment,Cost")] CreateOrderViewModel createOrderViewModel)
        {
            if (!string.IsNullOrEmpty(createOrderViewModel.DeliveryArea))
            {
                if (createOrderViewModel.DeliveryArea.EndsWith(", "))
                {
                    createOrderViewModel.DeliveryArea = createOrderViewModel.DeliveryArea.Remove(createOrderViewModel.DeliveryArea.Length - 2);
                }
            }

            if (!string.IsNullOrEmpty(Request["token1"]) && ModelState.IsValid)
            {
                HttpContext.Session["homePageModel1"] = createOrderViewModel;
                CreateOrderViewModel model = (CreateOrderViewModel)HttpContext.Session["homePageModel1"];
                Order order = this.ProcessOrder(createOrderViewModel);

                // Add the order to session for use when the customer returns from payment
                HttpContext.Session["orderInformation"] = order;

                var myCharge = new StripeChargeCreateOptions
                {
                    // convert the amount of $12.50 to cents i.e. 1250
                    Amount = (int)(order.Quote.Cost * 100),
                    Currency = "aud",
                    Description = "SG Fast Flyers Order Number:" + order.ID.ToString(),
                    SourceTokenOrExistingSourceId = Request["token1"]
                };


                var chargeService = new StripeChargeService(Config.privateStripeKey);

                var stripeCharge = chargeService.Create(myCharge);

                if (stripeCharge != null && !String.IsNullOrEmpty(stripeCharge.FailureMessage)) //// Failed.. 
                {
                    throw new Exception("ERROR:" + stripeCharge.FailureMessage);
                }
                else  //// All good
                {
                    string attach = Server.MapPath(@"\Content\Documents\SGFastFlyers_Letterbox_Printing_&_Delivery_Details.pdf");
                    var body = "Hi {6}, </br><p>Here is your order: </p></br><p>Order ID: {9}</p></br><p>Quantity: {0}</p><p>Delivery Date: {8}</p><p>Delivery Area: {7}</p><p>Metro Area: {1}</p><p>Is printing required: {2}" +
                        "</p><p>Print Size: {3}</p><p>Double Sided: {4}</p><p>Price: {5}</p></br><p>Thank you for your order.</p><p>Kind Regards,</p>SG Fast Flyers.";
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(model.EmailAddress));  // replace with valid value
                    message.Bcc.Add(new MailAddress(Config.sgEmail));
                    message.From = new MailAddress(Config.sgEmail);  // replace with valid value
                    message.Subject = "Order ID " + order.ID.ToString(); ;
                    message.Body = string.Format(body, model.Quantity, (model.IsMetro == true ? Config.Yes : Config.No), (model.NeedsPrint == true ? Config.Yes : Config.No), model.PrintSize, 
                        (model.IsDoubleSided == true ? Config.Yes : Config.No), model.FormattedCost, model.FirstName, model.DeliveryArea, model.DeliveryDate, order.ID);
                    message.IsBodyHtml = true;
                    message.Attachments.Add(new Attachment(attach));

                    try
                    {
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            await smtp.SendMailAsync(message);
                        }
                    }
                    catch (Exception)
                    {
                        //TODO: Something needs to be done with this exception...
                    }

                    return this.Redirect("/Orders/PaymentComplete");
                }
            }


            if (!string.IsNullOrEmpty(Request.Form["hdndirectDebitEmail"]) && ModelState.IsValid && createOrderViewModel.NeedsPrint)
            {
                HttpContext.Session["homePageModel1"] = createOrderViewModel;
                CreateOrderViewModel model = (CreateOrderViewModel)HttpContext.Session["homePageModel1"];
                DirectDebitEmail model1 = new DirectDebitEmail();

                Order order = this.ProcessOrder(createOrderViewModel);

                {
                    var url = @"\home\index";
                    var linkText = "Click here";
                    var body = "Hi {6}, </br><p>Here is your order: </p></br><p>Order ID: {9}</p></br><p>Quantity: {0}</p><p>Delivery Date: {8}</p><p>Delivery Area: {7}</p><p>Metro Area: {1}</p><p>Is printing required: {2}" +
                        "</p><p>Print Size: {3}</p><p>Double Sided: {4}</p><p>Price: {5}</p></br><p>Thank you for your order.</p><p> Our Direct Deposit Details are:</p><p>BSB: 014-289</p><p>" +
                        "Account: 463-181-792</p><p>Please use your name as your reference.</p><p>Kind Regards,</p>SG Fast Flyers.";
                    var firstName = model.FirstName;
                    string fN = string.Format("Hi {0},", firstName);
                    string href = String.Format("<a href='{0}'>{1}</a>", url, linkText);
                    string attach = Server.MapPath(@"\Content\Documents\SGFastFlyers_Letterbox_Printing_&_Delivery_Details.pdf");
                    string yourEncodedHtml = fN + "</br><p>You have been emailed your order with details of how to pay via Direct Debit. </p></br><p> Please note, your order " +
                        "will not be acted upon until payment has been recieved into our account.</p></br> <p>Thank you for your order.</p>" + href + " to begin another quote. <p>Have a great day.</p>";
                    var html = new MvcHtmlString(yourEncodedHtml);
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(createOrderViewModel.EmailAddress));  // replace with valid value
                    message.Bcc.Add(new MailAddress(Config.sgEmail));
                    message.From = new MailAddress(Config.sgEmail);  // replace with valid value
                    message.Subject = "Order ID " + order.ID.ToString();
                    message.Body = string.Format(body, model.Quantity, (model.IsMetro == true ? Config.Yes : Config.No), (model.NeedsPrint == true ? Config.Yes : Config.No), model.PrintSize, 
                        (model.IsDoubleSided == true ? Config.Yes : Config.No), model.FormattedCost, model.FirstName, model.DeliveryArea, model.DeliveryDate, order.ID);
                    message.IsBodyHtml = true;
                    message.Attachments.Add(new Attachment(attach));

                    try
                    {
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            await smtp.SendMailAsync(message);
                        }

                        ViewBag.Status = html;
                    }
                    catch (Exception)
                    {
                        ViewBag.Status = "Problem while sending email, Please check details.";
                    }
                }

                return View("DirectDebitEmail", model1);
            }

            if (!string.IsNullOrEmpty(Request.Form["hdndirectDebitEmail"]) && ModelState.IsValid && (createOrderViewModel.NeedsPrint == false))
            {
                HttpContext.Session["homePageModel1"] = createOrderViewModel;
                CreateOrderViewModel model = (CreateOrderViewModel)HttpContext.Session["homePageModel1"];
                DirectDebitEmail model1 = new DirectDebitEmail();


                Order order = this.ProcessOrder(createOrderViewModel);
                {

                    var url = @"\home\index";
                    var linkText = "Click here";
                    var body = "Hi {0}, </br><p>Here is your order: </p></br><p>Order ID: {6}</p></br><p>Quantity: {1}</p><p>Delivery Date: {2:d}</p><p>Delivery Area: {3}</p><p>Metro Area: {4}</p>" +
                        "<p>Price: {5}</p></br><p>Thank you for your order.</p><p> Our Direct Deposit Details are:</p><p>BSB: 014-289</p><p>" +
                        "Account: 463-181-792</p><p>Please use your name as your reference.</p><p>Kind Regards,</p>SG Fast Flyers.";
                    var firstName = model.FirstName;
                    string fN = string.Format("Hi {0},", firstName);
                    string href = String.Format("<a href='{0}'>{1}</a>", url, linkText);
                    string attach = Server.MapPath(@"\Content\Documents\SGFastFlyers_Letterbox_Printing_&_Delivery_Details.pdf");
                    string yourEncodedHtml = fN + "</br><p>You have been emailed your order with details of how to pay via Direct Debit. </p></br><p> Please note, your order " +
                        "will not be acted upon until payment has been recieved into our account.</p></br> <p>Thank you for your order.</p>" + href + " to begin another quote. <p>Have a great day.</p>";
                    var html = new MvcHtmlString(yourEncodedHtml);
                    var message = new MailMessage();
                    message.To.Add(new MailAddress(createOrderViewModel.EmailAddress));  // replace with valid value
                    message.Bcc.Add(new MailAddress(Config.sgEmail));
                    message.From = new MailAddress(Config.sgEmail);  // replace with valid value
                    message.Subject = "Your Quote";
                    message.Body = string.Format(body, model.FirstName, model.Quantity, model.DeliveryDate, model.DeliveryArea, (model.IsMetro == true ? Config.Yes : Config.No), 
                        model.FormattedCost, order.ID);
                    message.IsBodyHtml = true;
                    message.Attachments.Add(new Attachment(attach));

                    try
                    {
                        using (SmtpClient smtp = new SmtpClient())
                        {
                            await smtp.SendMailAsync(message);
                        }

                        ViewBag.Status = html;
                    }
                    catch (Exception)
                    {
                        ViewBag.Status = "Problem while sending email, Please check details.";
                    }
                }

                return View("DirectDebitEmail", model1);
            }

            return this.View(createOrderViewModel);
        }


        /// <summary>
        /// Handles completed payment processing.
        /// </summary>
        /// <param name="accessCode">The access code returned by the payment gateway, used to get payment details</param>
        /// <returns>Complete page view</returns>
        public ActionResult PaymentComplete(string accessCode)
        {
            bool paymentSuccess = true;

            if (HttpContext.Session["orderInformation"] != null)
            {
                Order completedOrder = this.db.Orders.Find(((Order)HttpContext.Session["orderInformation"]).ID);
                completedOrder.IsPaid = paymentSuccess;
                this.db.SaveChanges();
                return this.View(completedOrder);
            }

            return this.View();
        }

        /// <summary>
        /// GET: Orders/Edit/5
        /// </summary>
        /// <param name="id">the id to edit</param>
        /// <returns>The edit page for an order</returns>
        [Authorize(Roles = "Admin")] // TODO: This will have to be changed when users are allowed to edit their orders
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = this.db.Orders.Find(id);
            if (order == null)
            {
                return this.HttpNotFound();
            }

            ViewBag.ID = new SelectList(this.db.DeliveryDetails, "ID", "DeliveryArea", order.ID);
            ViewBag.ID = new SelectList(this.db.PrintDetails, "ID", "ID", order.ID);
            return this.View(order);
        }

        /// <summary>
        /// POST: Orders/Edit/5
        /// </summary>
        /// <param name="order">The order being edited</param>
        /// <returns>View of the order</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // TODO: This will have to be changed when users are allowed to edit their orders
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,EmailAddress,PhoneNumber,Quantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                this.db.Entry(order).State = EntityState.Modified;
                this.db.SaveChanges();
                return this.RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(this.db.DeliveryDetails, "ID", "DeliveryArea", order.ID);
            ViewBag.ID = new SelectList(this.db.PrintDetails, "ID", "ID", order.ID);
            return this.View(order);
        }

        /// <summary>
        /// GET: Orders/Delete/5
        /// </summary>
        /// <param name="id">the id of the order</param>
        /// <returns>The get view for deleting an order</returns>
        [Authorize(Roles = "Admin")] // TODO: This will have to be changed when users are allowed to edit their orders
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = this.db.Orders.Find(id);
            if (order == null)
            {
                return this.HttpNotFound();
            }

            return this.View(order);
        }

        /// <summary>
        /// Deletes an order
        /// </summary>
        /// <param name="id">the id to delete</param>
        /// <returns>A redirect to the index page</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // TODO: This will have to be changed when users are allowed to edit their orders
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = this.db.Orders.Find(id);
            this.db.Orders.Remove(order);
            this.db.SaveChanges();
            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetDistributionList(string searchTerm)
        {
            var DistributionModel = new DistributionListModels();
            DataTable dt = ReadDistributionList(searchTerm, false);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    DistributionModel.DistributionList.Add(new DistributionListModels()
                    {
                        DeliveryArea = row["Column3"].ToString() + " " + row["Column6"].ToString(),
                        Region = row["Column2"].ToString()
                    });
                }
            }
            return Json(new { success = true, DistributionModel, message = "successfully." }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTotalUnits(string searchTerm, bool units = true)
        {
            string postcodes = string.Empty;
            int totalUnits = 0;
            bool isCountry = false;
            DataTable dt = ReadDistributionListUnits(searchTerm, units);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    postcodes = string.Join(", ", row["Column6"].ToString());
                    totalUnits += int.Parse(row["Column11"].ToString());
                    isCountry = row["Column2"].ToString() == "Country" ? true : false;
                }
            }
            return Json(new { success = true, totalUnits, country = isCountry, postcodes,
                message = "successfully." }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Read the Excel Distribution List
        /// </summary>
        public DataTable ReadDistributionList(string sLookUpString, bool units)
        {
            DataTable dt = new DataTable();
            DataSet result = HttpContext.Cache["sk"] as DataSet;
            string path = string.Format("{0}", Server.MapPath(Config.ExcelFilePath));

            try
            {
                if (!System.IO.File.Exists(path))
                {
                    return null;
                }
                else
                {
                    if (HttpContext.Cache["sk"] == null)
                    {
                        FileStream stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        excelReader.IsFirstRowAsColumnNames = false;

                        result = excelReader.AsDataSet();
                        if (!units)
                        {
                            dt = IO.ConvertXSLXtoDataTable(result.Tables[1], sLookUpString);
                        }
                        else
                        {
                            dt = IO.ConvertXSLXtoDataTableUnits(result.Tables[1], sLookUpString);
                        }
                        HttpContext.Cache.Insert("sk", result, null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration);
                        excelReader.Close();
                    }
                    else
                    {
                        if (!units)
                        {
                            dt = IO.ConvertXSLXtoDataTable(result.Tables[1], sLookUpString);
                        }
                        else
                        {
                            dt = IO.ConvertXSLXtoDataTableUnits(result.Tables[1], sLookUpString);
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
            }
            return dt;
        }

        /// <summary>
        /// Read the Excel Distribution List
        /// </summary>
        public DataTable ReadDistributionListUnits(string sLookUpString, bool units)
        {
            DataTable dt = new DataTable();
            DataSet resultUnits = HttpContext.Cache["units"] as DataSet;
            string path = string.Format("{0}", Server.MapPath(Config.ExcelFilePath));

            try
            {
                if (!System.IO.File.Exists(path))
                {
                    return null;
                }
                else
                {
                    if (HttpContext.Cache["units"] == null)
                    {
                        FileStream stream = System.IO.File.Open(path, FileMode.Open, FileAccess.Read);
                        excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        excelReader.IsFirstRowAsColumnNames = false;

                        resultUnits = excelReader.AsDataSet();
                        if (!units)
                        {
                            dt = IO.ConvertXSLXtoDataTable(resultUnits.Tables[1], sLookUpString);
                        }
                        else
                        {
                            dt = IO.ConvertXSLXtoDataTableUnits(resultUnits.Tables[1], sLookUpString);
                        }
                        HttpContext.Cache.Insert("units", resultUnits, null, DateTime.Now.AddHours(2), System.Web.Caching.Cache.NoSlidingExpiration);
                        excelReader.Close();
                    }
                    else
                    {
                        if (!units)
                        {
                            dt = IO.ConvertXSLXtoDataTable(resultUnits.Tables[1], sLookUpString);
                        }
                        else
                        {
                            dt = IO.ConvertXSLXtoDataTableUnits(resultUnits.Tables[1], sLookUpString);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return dt;
        }

        /// <summary>
        /// Disposes the thing
        /// </summary>
        /// <param name="disposing">whether it is disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Creates the order and associated details, print, delivery quote.
        /// </summary>
        /// <param name="createOrderViewModel">The new order in view model form</param>
        /// <returns>The created order</returns>
        private Order ProcessOrder(CreateOrderViewModel createOrderViewModel)
        {
            Order order = new Order
            {
                FirstName = createOrderViewModel.FirstName,
                LastName = createOrderViewModel.LastName,
                EmailAddress = createOrderViewModel.EmailAddress,
                PhoneNumber = createOrderViewModel.PhoneNumber,
                Quantity = createOrderViewModel.Quantity
            };

            this.db.Orders.Add(order);
            this.db.SaveChanges();

            DeliveryDetail deliveryDetail = new DeliveryDetail
            {
                OrderID = order.ID,
                DeliveryArea = createOrderViewModel.DeliveryArea,
                DeliveryDate = createOrderViewModel.DeliveryDate
            };

            this.db.DeliveryDetails.Add(deliveryDetail);

            PrintDetail printDetail = new PrintDetail
            {
                OrderID = order.ID,
                NeedsPrint = createOrderViewModel.NeedsPrint,
                PrintFormat = createOrderViewModel.PrintFormat,
                PrintSize = createOrderViewModel.PrintSize
            };

            this.db.PrintDetails.Add(printDetail);
            if (createOrderViewModel.Attachment != null)
            {
                string pathToSave = Server.MapPath(Config.objectDataPath) + "\\Order\\" + order.ID.ToString() + "\\";
                IO.CreateFolder(pathToSave);
                createOrderViewModel.Attachment.SaveAs(pathToSave + Path.GetFileName(createOrderViewModel.Attachment.FileName));
            }


            Quote quote = new Quote
            {
                OrderID = order.ID,
                Cost = createOrderViewModel.Cost,
                IsMetro = createOrderViewModel.IsMetro,
                Quantity = createOrderViewModel.Quantity
            };

            this.db.Quotes.Add(quote);
            this.db.SaveChanges();

            return order;
        }
    }
}
