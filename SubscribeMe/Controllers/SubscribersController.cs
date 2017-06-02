using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SubscribeMe.Models;

namespace SubscribeMe.Controllers
{
    public class SubscribersController : Controller
    {
        private Model1Container db = new Model1Container();

        public ActionResult Download()
        {
          ExportToXML();
            return Content("Thanks for downloading.");

        }

        // GET: Subscribers
        public ActionResult Index()
        {
            return View(db.Subscribers.ToList());
        }

        // GET: Subscribers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscribers subscribers = db.Subscribers.Find(id);
            if (subscribers == null)
            {
                return HttpNotFound();
            }
            return View(subscribers);
        }

        // GET: Subscribers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subscribers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Number")] Subscribers subscribers)
        {
            if (ModelState.IsValid)
            {
                subscribers.DateSubscribed = DateTime.Now.ToShortDateString();
                db.Subscribers.Add(subscribers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscribers);
        }

        // GET: Subscribers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscribers subscribers = db.Subscribers.Find(id);
            if (subscribers == null)
            {
                return HttpNotFound();
            }
            return View(subscribers);
        }

        // POST: Subscribers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Number,DateSubscribed")] Subscribers subscribers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subscribers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subscribers);
        }

        // GET: Subscribers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subscribers subscribers = db.Subscribers.Find(id);
            if (subscribers == null)
            {
                return HttpNotFound();
            }
            return View(subscribers);
        }

        // POST: Subscribers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subscribers subscribers = db.Subscribers.Find(id);
            db.Subscribers.Remove(subscribers);
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

        public void ExportToXML()
        {
            var data = db.Subscribers.ToList();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=subscribers.xml"); Response.ContentType = "text/xml";
            var serializer = new
 System.Xml.Serialization.XmlSerializer(data.GetType()); serializer.Serialize(Response.OutputStream, data);
        }

    }
}
