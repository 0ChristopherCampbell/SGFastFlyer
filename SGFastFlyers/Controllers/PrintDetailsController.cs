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
    public class PrintDetailsController : Controller
    {
        private OrderContext db = new OrderContext();

        // GET: PrintDetails
        public ActionResult Index()
        {
            var printDetails = db.PrintDetails.Include(p => p.Order);
            return View(printDetails.ToList());
        }

        // GET: PrintDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrintDetail printDetail = db.PrintDetails.Find(id);
            if (printDetail == null)
            {
                return HttpNotFound();
            }
            return View(printDetail);
        }

        // GET: PrintDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrintDetail printDetail = db.PrintDetails.Find(id);
            if (printDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Orders, "ID", "FirstName", printDetail.ID);
            return View(printDetail);
        }

        // POST: PrintDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,NeedsPrint,PrintSize,IsDoubleSided")] PrintDetail printDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(printDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Orders, "ID", "FirstName", printDetail.ID);
            return View(printDetail);
        }

        // GET: PrintDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PrintDetail printDetail = db.PrintDetails.Find(id);
            if (printDetail == null)
            {
                return HttpNotFound();
            }
            return View(printDetail);
        }

        // POST: PrintDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PrintDetail printDetail = db.PrintDetails.Find(id);
            db.PrintDetails.Remove(printDetail);
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
