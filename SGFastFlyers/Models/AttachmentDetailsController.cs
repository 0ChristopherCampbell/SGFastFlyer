using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace SGFastFlyers.Models
{
    public class AttachmentDetailsController : Controller
    {
        // GET: AttachmentDetails
        private DataAccessLayer.SGDbContext db = new DataAccessLayer.SGDbContext();

        // GET: PrintDetails
        public ActionResult Index()
        {
            var attachmentDetails = db.AttachmentDetails.Include(p => p.Order);
            return View(attachmentDetails.ToList());
        }

        // GET: PrintDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttachmentDetail attachmentDetail = db.AttachmentDetails.Find(id);
            if (attachmentDetail == null)
            {
                return HttpNotFound();
            }
            return View(attachmentDetail);
        }

        // GET: PrintDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttachmentDetail attachmentDetail = db.AttachmentDetails.Find(id);
            if (attachmentDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Orders, "ID", "File", attachmentDetail.ID);
            return View(attachmentDetail);
        }

        // POST: PrintDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,OrderID,FileName,File")] AttachmentDetail attachmentDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attachmentDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.Orders, "ID", "File", attachmentDetail.ID);
            return View(attachmentDetail);
        }

        // GET: PrintDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttachmentDetail attachmentDetail = db.AttachmentDetails.Find(id);
            if (attachmentDetail == null)
            {
                return HttpNotFound();
            }
            return View(attachmentDetail);
        }

        // POST: PrintDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AttachmentDetail attachmentDetail = db.AttachmentDetails.Find(id);
            db.AttachmentDetails.Remove(attachmentDetail);
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
