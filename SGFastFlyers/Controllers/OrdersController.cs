using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SGFastFlyers.DataAccessLayer;
using SGFastFlyers.Models;
using SGFastFlyers.ViewModels;

namespace SGFastFlyers.Controllers
{
    public class OrdersController : Controller
    {
        private OrderContext db = new OrderContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.DeliveryDetail).Include(o => o.PrintDetail);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
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
        public ActionResult Create(bool? prepopulated)
        {
            bool prePopulated;
            if (bool.TryParse(prepopulated.ToString(), out prePopulated))
            {
                if (HttpContext.Session["instantQuoteOrder"] != null && prePopulated)
                {
                    CreateOrderViewModel newOrder = (CreateOrderViewModel)HttpContext.Session["instantQuoteOrder"];
                    return View(newOrder);
                }
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

        // GET: Orders/Edit/5
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

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
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
