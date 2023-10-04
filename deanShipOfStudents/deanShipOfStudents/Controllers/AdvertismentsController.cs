using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using deanShipOfStudents.Models;

namespace deanShipOfStudents.Controllers
{
    [Authorize]
    public class AdvertismentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Advertisments
        public ActionResult Index()
        {
            return View(db.Advertisments.ToList());
        }
        [HttpPost]
        public ActionResult sendToTrainee(int id)
        {
            Advertisment advertisment = db.Advertisments.Find(id);
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            mail.From = new MailAddress("seniorprojectkau1999@gmail.com");
            try
            {

                foreach (var user in db.Users.Where(u => u.userType == ApplicationUser.UserType.trainee && u.isActive))
                {

                    mail.To.Add(user.Email);
                    mail.Subject = advertisment.title;
                    mail.Body = advertisment.body;
                    mail.IsBodyHtml = true;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("seniorprojectkau1999@gmail.com", "Senior@Project99");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
            catch (Exception e)
            {

                ViewBag.error = e.Message;
            }

            return View();
        }

        // GET: Advertisments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisment advertisment = db.Advertisments.Find(id);
            if (advertisment == null)
            {
                return HttpNotFound();
            }
            return View(advertisment);
        }

        // GET: Advertisments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Advertisments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Advertisment advertisment,HttpPostedFileBase UploadFile)
        {
            try
            {
                string DirectoryPath = Server.MapPath("~/UploadedFiles/");
                if (UploadFile != null)
                {
                    if (!Directory.Exists(DirectoryPath))
                    {
                        Directory.CreateDirectory(DirectoryPath);
                    }
                    var ext = Path.GetExtension(UploadFile.FileName);
                    var newName = "file_" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second  + ext;
                    var ServerSavePath = Path.Combine(DirectoryPath + newName);
                    UploadFile.SaveAs(ServerSavePath);
                    advertisment.Attatchment = newName;
                    db.Advertisments.Add(advertisment);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception e)
            {

                ModelState.AddModelError("", e.Message);
            }
          
            return View(advertisment);
        }

        // GET: Advertisments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisment advertisment = db.Advertisments.Find(id);
            if (advertisment == null)
            {
                return HttpNotFound();
            }
            return View(advertisment);
        }

        // POST: Advertisments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,body,Attatchment")] Advertisment advertisment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advertisment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(advertisment);
        }

        // GET: Advertisments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advertisment advertisment = db.Advertisments.Find(id);
            if (advertisment == null)
            {
                return HttpNotFound();
            }
            return View(advertisment);
        }

        // POST: Advertisments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advertisment advertisment = db.Advertisments.Find(id);
            db.Advertisments.Remove(advertisment);
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
