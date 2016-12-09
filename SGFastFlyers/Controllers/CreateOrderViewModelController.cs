using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SGFastFlyers.DataAccessLayer;
using SGFastFlyers.ViewModels;
using SGFastFlyers.Models;

namespace SGFastFlyers.Controllers
{
    public class CreateOrderViewModelController : Controller
    {
        private DataAccessLayer.SGDbContext db = new DataAccessLayer.SGDbContext();

        // GET: CreateOrderViewModel/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(bool prePopulated)
        {
            if (HttpContext.Session["instantQuoteOrder"] != null && prePopulated)
            {
                CreateOrderViewModel newOrder = (CreateOrderViewModel)HttpContext.Session["instantQuoteOrder"];
                return View(newOrder);
            }
            else
            {
                return View();
            }
        }

        // POST: CreateOrderViewModel/Create
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
                    Cost = createOrderViewModel.Cost,
                    IsMetro = createOrderViewModel.IsMetro,
                    Quantity = createOrderViewModel.Quantity,
                    OrderID = order.ID
                };
                db.Quotes.Add(quote);

                db.SaveChanges();

                return RedirectToAction("Index", "Orders");
            }

            return View(createOrderViewModel);
        }

        // GET: CreateOrderViewModel/Edit/5
        /*public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CreateOrderViewModel createOrderViewModel = db.CreateOrderViewModels.Find(id);
            if (createOrderViewModel == null)
            {
                return HttpNotFound();
            }
            return View(createOrderViewModel);
        }*/

        // POST: CreateOrderViewModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,EmailAddress,PhoneNumber,Quantity,DeliveryDate,DeliveryArea,NeedsPrint,PrintSize,IsDoubleSided")] CreateOrderViewModel createOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(createOrderViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(createOrderViewModel);
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
