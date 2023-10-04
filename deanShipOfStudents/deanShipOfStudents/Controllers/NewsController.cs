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
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Main()
        {
            return View(db.News.Where(a=>a.isPublished).ToList());

        }


        // GET: News
        public ActionResult Index()
        {
            return View(db.News.ToList().OrderByDescending(a=>a.Id));
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( News news , HttpPostedFileBase UploadFile)
        {
            try
            {
                string DirectoryPath = Server.MapPath("~/NewsImages/");
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

                    news.FileName = newName;
                   
                    db.News.Add(news);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    return View(news);
                }

            }
            catch (Exception)
            {

                return View(news);
            }

          

           
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( News news, HttpPostedFileBase UploadFile)
        {
            try
            {
                string DirectoryPath = Server.MapPath("~/NewsImages/");
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

                    news.FileName = newName;

                    db.Entry(news).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                else
                {
                    db.Entry(news).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception)
            {

                return View(news);
            }
           
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
