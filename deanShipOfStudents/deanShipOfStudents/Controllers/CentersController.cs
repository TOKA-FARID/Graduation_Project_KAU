using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using deanShipOfStudents.Models;

namespace deanShipOfStudents.Controllers
{
    [Authorize]

    public class CentersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Centers
        public ActionResult Index()
        {
            var centers = db.Centers.Include(c => c.Supervisor);
            return View(centers.ToList());
        }

        // GET: Centers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Center center = db.Centers.Include(c => c.Supervisor).Where(c=>c.CenterId == id).ToList().FirstOrDefault();
            if (center == null)
            {
                return HttpNotFound();
            }
            return View(center);
        }

        // GET: Centers/Create
        public ActionResult Create()
        {
            ViewBag.SupervisorId = new SelectList(db.Users.Where(u=>u.userType == ApplicationUser.UserType.superVisor && u.isActive), "Id", "firstName");
            return View();
        }

        // POST: Centers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CenterId,CenterName,Description,Objective,SupervisorId")] Center center)
        {
            if (ModelState.IsValid)
            {
                db.Centers.Add(center);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupervisorId = new SelectList(db.Users.Where(u => u.userType == ApplicationUser.UserType.superVisor && u.isActive), "Id", "firstName", center.SupervisorId);
            return View(center);
        }

        // GET: Centers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Center center = db.Centers.Find(id);
            if (center == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupervisorId = new SelectList(db.Users.Where(u => u.userType == ApplicationUser.UserType.superVisor && u.isActive), "Id", "firstName", center.SupervisorId);
            return View(center);
        }

        // POST: Centers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CenterId,CenterName,Description,Objective,SupervisorId")] Center center)
        {
            if (ModelState.IsValid)
            {
                db.Entry(center).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupervisorId = new SelectList(db.Users.Where(u => u.userType == ApplicationUser.UserType.superVisor && u.isActive), "Id", "firstName", center.SupervisorId);
            return View(center);
        }

        // GET: Centers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Center center = db.Centers.Include(c=>c.Supervisor).Where(c=>c.CenterId == id).ToList().FirstOrDefault();
            if (center == null)
            {
                return HttpNotFound();
            }
            return View(center);
        }

        // POST: Centers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Center center = db.Centers.Find(id);
            db.Centers.Remove(center);
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
