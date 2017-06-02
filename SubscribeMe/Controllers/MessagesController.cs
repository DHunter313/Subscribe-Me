using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SubscribeMe.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SubscribeMe.Controllers
{


    public class MessagesController : Controller
    {
        
        // Find your Account Sid and Auth Token at twilio.com/console
        const string accountSid = "AC9b46119ade4cd6836865f6890ec815c0";
        const string authToken = "f12c3a8e7bf03bfd01d5b4ccc8f6198a";

        private Model1Container db = new Model1Container();

        // GET: Messages
        public ActionResult Index()
        {

            return View(db.Messages.ToList().OrderBy(x => x.SentDate));


        }
        public ActionResult Download()
        {
            ExportToXML();
            return Content("Thanks for downloading.");

        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {



            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MessageSent,ToNumber,ToName")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                string num = "+1" + messages.ToNumber;

                messages.SentDate = DateTime.Now.ToString();
                db.Messages.Add(messages);
                db.SaveChanges();



                TwilioClient.Init(accountSid, authToken);
                var to = new PhoneNumber(num);
                var message = MessageResource.Create(
                    to,
                    from: new PhoneNumber("+13139864587"),
                    body: messages.MessageSent);

                return Content("Thank you. your message has been sent.");
            }

            return View(messages);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MessageSent,SentDate,ToNumber,ToName")] Messages messages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(messages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(messages);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Messages messages = db.Messages.Find(id);
            if (messages == null)
            {
                return HttpNotFound();
            }
            return View(messages);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Messages messages = db.Messages.Find(id);
            db.Messages.Remove(messages);
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
            var data = db.Messages.ToList();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Sent_messages.xml"); Response.ContentType = "text/xml";
            var serializer = new
 System.Xml.Serialization.XmlSerializer(data.GetType()); serializer.Serialize(Response.OutputStream, data);
        }
    }
}
