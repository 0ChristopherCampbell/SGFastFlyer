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
    using System.IO;
    using System.Web;
    using System.Web.Mvc;

    using ViewModels;

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
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,EmailAddress,PhoneNumber,Quantity,DeliveryDate,DeliveryArea,NeedsPrint,PrintSize,IsDoubleSided,Attachment")] CreateOrderViewModel createOrderViewModel)
        {
            // This shouldn't be being used at the moment..
            Exception exception = new Exception("This shouldn't be used directly");
            return View(createOrderViewModel);
            throw (exception);
            
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
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,EmailAddress,PhoneNumber,Quantity,DeliveryDate,DeliveryArea,NeedsPrint,PrintSize,IsDoubleSided,Attachment")] CreateOrderViewModel createOrderViewModel)
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
