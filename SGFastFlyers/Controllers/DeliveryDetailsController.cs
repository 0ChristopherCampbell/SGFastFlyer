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

namespace SGFastFlyers.Controllers
{
    public class DeliveryDetailsController : Controller
    {
        private OrderContext db = new OrderContext();

        // GET: DeliveryDetails
        public ActionResult Index()
        {
            var deliveryDetails = db.DeliveryDetails.Include(d => d.Order);
            return View(deliveryDetails.ToList());
        }

        // GET: DeliveryDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryDetail deliveryDetail = db.DeliveryDetails.Find(id);
            if (deliveryDetail == null)
            {
                return HttpNotFound();
            }
            return View(deliveryDetail);
        }

        // GET: DeliveryDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryDetail deliveryDetail = db.DeliveryDetails.Find(id);
            if (deliveryDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Orders, "ID", "FirstName", deliveryDetail.ID);
            return View(deliveryDetail);
        }

        // POST: DeliveryDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,DeliveryDate,DeliveryArea")] DeliveryDetail deliveryDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Orders, "ID", "FirstName", deliveryDetail.ID);
            return View(deliveryDetail);
        }

        // GET: DeliveryDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryDetail deliveryDetail = db.DeliveryDetails.Find(id);
            if (deliveryDetail == null)
            {
                return HttpNotFound();
            }
            return View(deliveryDetail);
        }

        // POST: DeliveryDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryDetail deliveryDetail = db.DeliveryDetails.Find(id);
            db.DeliveryDetails.Remove(deliveryDetail);
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
