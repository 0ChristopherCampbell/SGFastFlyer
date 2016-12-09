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
    using ViewModels;
    using Utility;

    using eWAY.Rapid;
    using eWAY.Rapid.Models;
    using eWAY.Rapid.Enums;

    [RequireHttps]
    public class OrdersController : Controller
    {
        private SGDbContext db = new SGDbContext();
        
        // GET: Orders
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.DeliveryDetail).Include(o => o.PrintDetail);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }
        

        // GET: Orders/Create?prepopulated=bool
        [HttpGet]
        public ActionResult Create(bool prepopulated = false)
        {            
            if (HttpContext.Session["homePageModel"] != null && prepopulated)
            {
                InstantQuoteViewModel model = (InstantQuoteViewModel)HttpContext.Session["homePageModel"];
                CreateOrderViewModel orderModel = new CreateOrderViewModel
                {
                    Cost = model.Cost,
                    NeedsPrint = model.NeedsPrint,
                    PrintFormat = model.PrintFormat,
                    IsMetro = model.IsMetro,
                    Quantity = model.Quantity,
                    PrintSize = model.PrintSize,
                    DeliveryDate = DateTime.Now.AddDays(7)
                };

                return View(orderModel);
            }            

            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,EmailAddress,PhoneNumber,Quantity,DeliveryDate,DeliveryArea,NeedsPrint,PrintSize,IsDoubleSided")] CreateOrderViewModel createOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order
                {
                    FirstName = createOrderViewModel.FirstName,
                    LastName = createOrderViewModel.LastName,
                    EmailAddress = createOrderViewModel.EmailAddress,
                    PhoneNumber = createOrderViewModel.PhoneNumber,
                    Quantity = createOrderViewModel.Quantity
                };

                db.Orders.Add(order);
                db.SaveChanges();

                DeliveryDetail deliveryDetail = new DeliveryDetail
                {
                    OrderID = order.ID,
                    DeliveryArea = createOrderViewModel.DeliveryArea,
                    DeliveryDate = createOrderViewModel.DeliveryDate
                };

                db.DeliveryDetails.Add(deliveryDetail);

                PrintDetail printDetail = new PrintDetail
                {
                    OrderID = order.ID,
                    NeedsPrint = createOrderViewModel.NeedsPrint,
                    PrintFormat = createOrderViewModel.PrintFormat,
                    PrintSize = createOrderViewModel.PrintSize
                };

                db.PrintDetails.Add(printDetail);

                Quote quote = new Quote
                {
                    OrderID = order.ID,
                    Cost = createOrderViewModel.Cost,
                    IsMetro = createOrderViewModel.IsMetro,
                    Quantity = createOrderViewModel.Quantity                    
                };

                db.Quotes.Add(quote);
                db.SaveChanges();

                // Add the order to session for use when the customer returns from payment
                HttpContext.Session["orderInformation"] = order;

                #region Payment Processing

                IRapidClient ewayClient = RapidClientFactory.NewRapidClient(Config.apiPaymentKey, Config.apiPaymentPassword, Config.apiRapidEndpoint);

                Transaction transaction = new Transaction();

                PaymentDetails paymentDetails = new PaymentDetails();
                paymentDetails.TotalAmount = (int)(quote.Cost*100); // Convert to integer form currency (cents)

                transaction.PaymentDetails = paymentDetails;
                transaction.RedirectURL = "https://localhost:44300/Orders/PaymentComplete"; // Needs to be changed for live
                transaction.CancelURL = "http://www.eway.com.au";
                transaction.TransactionType = TransactionTypes.Purchase;

                CreateTransactionResponse response = ewayClient.Create(PaymentMethod.ResponsiveShared, transaction);

                if (response.Errors != null)
                {
                    foreach (string errorCode in response.Errors)
                    {
                        Console.WriteLine("Error Message: " + RapidClientFactory.UserDisplayMessage(errorCode, "EN"));
                    }
                }

                return Redirect(response.SharedPaymentUrl);
                #endregion

                //return RedirectToAction("Index", "Orders");
            }

            return View(createOrderViewModel);
        }
        
        /// <summary>
        /// Handles completed payment processing.
        /// </summary>
        /// <param name="AccessCode">The access code returned by the payment gateway, used to get payment details</param>
        /// <returns>Complete page view</returns>
        public ActionResult PaymentComplete(string AccessCode)
        {
            IRapidClient ewayClient = RapidClientFactory.NewRapidClient(Config.apiPaymentKey, Config.apiPaymentPassword, Config.apiRapidEndpoint);
            bool paymentSuccess = false;
            QueryTransactionResponse response = ewayClient.QueryTransaction(AccessCode);
            if ((bool)response.TransactionStatus.Status)
            {
                paymentSuccess = true;
                Console.WriteLine("Payment successful! ID: " + response.TransactionStatus.TransactionID);
            }
            else
            {
                string[] errorCodes = response.TransactionStatus.ProcessingDetails.ResponseMessage.Split(new[] { ", " }, StringSplitOptions.None);
                foreach (string errorCode in errorCodes)
                {
                    Console.WriteLine("Error Message: " + RapidClientFactory.UserDisplayMessage(errorCode, "EN"));
                }
            }

            if (HttpContext.Session["orderInformation"] != null)
            {
                Order completedOrder = db.Orders.Find(((Order)HttpContext.Session["orderInformation"]).ID);
                completedOrder.IsPaid = paymentSuccess;
                db.SaveChanges();
                return View(completedOrder);
            }

            return View();
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin")] // TODO: This will have to be changed when users are allowed to edit their orders
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.DeliveryDetails, "ID", "DeliveryArea", order.ID);
            ViewBag.ID = new SelectList(db.PrintDetails, "ID", "ID", order.ID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // TODO: This will have to be changed when users are allowed to edit their orders
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,EmailAddress,PhoneNumber,Quantity")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.DeliveryDetails, "ID", "DeliveryArea", order.ID);
            ViewBag.ID = new SelectList(db.PrintDetails, "ID", "ID", order.ID);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin")] // TODO: This will have to be changed when users are allowed to edit their orders
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5 // TODO: This will have to be changed when users are allowed to edit their orders
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
