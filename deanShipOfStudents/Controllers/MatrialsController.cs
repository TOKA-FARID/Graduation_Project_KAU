using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using deanShipOfStudents.Models;

namespace deanShipOfStudents.Controllers
{
    [Authorize]

    public class MatrialsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index(int? activityId)
        {
            if (activityId != null)
            {
                ViewBag.activityId = activityId;
                return View(db.Matrials.Include(m => m.activity).Where(b => b.activityId == activityId).ToList());

            }
            else
            {
                return View(db.Matrials.Include(m => m.activity).ToList());

            }
        }
      

        // GET: Matrials/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matrial matrial = db.Matrials.Include(m => m.activity).Where(b => b.Id == id).ToList().FirstOrDefault();
            if (matrial == null)
            {
                return HttpNotFound();
            }
            return View(matrial);
        }

        // GET: Matrials/Create
        public ActionResult Create(int? activityId)
        {
            if (activityId != null )
            {
                ViewBag.activityId = new SelectList(db.Activities.Where(a=>a.ActivityId == activityId), "ActivityId", "ActivityName",activityId);

            }
            else
            {
                ViewBag.activityId = new SelectList(db.Activities, "ActivityId", "ActivityName");

            }
            return View();
        }

        // POST: Matrials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Matrial matrial , HttpPostedFileBase UploadFile)
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
                    //var InputFileName = Path.GetFileName(file.FileName);
                    var newName = "file" + DateTime.Now.Date.Year + DateTime.Now.Date.Month + DateTime.Now.Date.Day + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "." + ext;
                    var ServerSavePath = Path.Combine(DirectoryPath + newName);
                    UploadFile.SaveAs(ServerSavePath);

                    matrial.filePath = newName;
                    matrial.ext = ext;
                    db.Matrials.Add(matrial);
                    db.SaveChanges();
                    return RedirectToAction("Index", new { activityId = matrial.activityId });

                }
                else
                {
                    return View(matrial);
                }
               
            }
            catch (Exception)
            {

                ViewBag.activityId = new SelectList(db.Activities.Where(a => a.ActivityId == matrial.activityId), "ActivityId", "ActivityName", matrial.activityId);
                return View(matrial);
            }
               
           

          
        }

        // GET: Matrials/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matrial matrial = db.Matrials.Include(m => m.activity).Where(b => b.Id == id).ToList().FirstOrDefault();
            if (matrial == null)
            {
                return HttpNotFound();
            }
            ViewBag.activityId = new SelectList(db.Activities.Where(a => a.ActivityId == matrial.activityId), "ActivityId", "ActivityName", matrial.activityId);
            return View(matrial);
        }

        // POST: Matrials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Matrial matrial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(matrial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { activityId = matrial.activityId });
            }
            ViewBag.activityId = new SelectList(db.Activities.Where(a => a.ActivityId == matrial.activityId), "ActivityId", "ActivityName", matrial.activityId);
            return View(matrial);
        }

        // GET: Matrials/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Matrial matrial = db.Matrials.Include(m => m.activity).Where(b => b.Id == id).ToList().FirstOrDefault();
            if (matrial == null)
            {
                return HttpNotFound();
            }
            return View(matrial);
        }

        // POST: Matrials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Matrial matrial = db.Matrials.Include(m => m.activity).Where(b => b.Id == id).ToList().FirstOrDefault();
            db.Matrials.Remove(matrial);
            db.SaveChanges();
            return RedirectToAction("Index", new { activityId = matrial.activityId });
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
