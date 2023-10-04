using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using deanShipOfStudents.Models;
using LinqToExcel;

namespace deanShipOfStudents.Controllers
{
    [Authorize]

    public class AttatchmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Attatchments
        public ActionResult Index(int? activityId , Attatchment.FileType type)
        {
            List<Attatchment> attatchments = new List<Attatchment>();
            if (activityId != null)
            {
                    attatchments = db.Attatchments.Include(a => a.activity).Include(a => a.traine).Where(a => a.type == type && a.activityId == activityId).ToList();
                ViewBag.activityId = activityId;
                ViewBag.type = type; 
            }
            else
            {
                attatchments = db.Attatchments.Include(a => a.activity).Include(a => a.traine).Where(a => a.type == type ).ToList();
            }
            return View(attatchments);
        }

        public ActionResult UploadExcel(Attatchment.FileType type , int activityId)
        {
            ViewBag.filetype = type;
            switch (type)
            {
                case Attatchment.FileType.attendance:
                    ViewBag.type = "Attendance";
                    break;
                case Attatchment.FileType.evaluation:
                    ViewBag.type = "Evaluation";
                    break;
                default:
                    ViewBag.type = "Unknown";
                    break;
            }
            ViewBag.activityId = activityId;
            return View();
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase FileUpload, Attatchment.FileType type , int activityId)
        {
            List<string> data = new List<string>();

            try
            {
                if (FileUpload != null)
                {
                    // tdata.ExecuteCommand("truncate table OtherCompanyAssets");
                    if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string filename = FileUpload.FileName;
                        string targetpath = Server.MapPath("~/Doc/");
                        FileUpload.SaveAs(targetpath + filename);
                        string pathToExcelFile = targetpath + filename;
                        var connectionString = "";
                        if (filename.EndsWith(".xls"))
                        {
                            connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                        }
                        else if (filename.EndsWith(".xlsx"))
                        {
                            connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                        }
                       
                        
                            var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                            var ds = new DataSet();
                            adapter.Fill(ds, "ExcelTable");
                            DataTable dtable = ds.Tables["ExcelTable"];

                            var excelFile = new ExcelQueryFactory(pathToExcelFile);
                            var Students = from a in excelFile.Worksheet("Sheet1") select a;
                            var al = Students.ToList();
                           for (int a = 0; a < al.Count(); a++)
                                {
                                    try
                                    {
                                        Attatchment attatchment = new Attatchment();
                                        attatchment.type = type;
                                        string uniId = al[a][0];
                                        var user = db.Users.Where(u => u.userType == ApplicationUser.UserType.trainee && u.UniverstyId == uniId).ToList().FirstOrDefault();
                                        attatchment.traineId = user.Id;
                                        attatchment.degree = Convert.ToDouble(al[a][1]);
                                        attatchment.activityId = activityId;
                                        db.Attatchments.Add(attatchment);
                                        db.SaveChanges();
                                    }
                                    catch (DbEntityValidationException ex)
                                    {
                                        foreach (var entityValidationErrors in ex.EntityValidationErrors)
                                        {
                                            foreach (var validationError in entityValidationErrors.ValidationErrors)
                                            {
                                                data.Add("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                                            }
                                        }
                                        data.ToArray();
                                        return Json(data, JsonRequestBehavior.AllowGet);

                                    }
                                }

                          


                    
                        if ((System.IO.File.Exists(pathToExcelFile)))
                        {
                            System.IO.File.Delete(pathToExcelFile);
                        }
                        data.Add("<p>File Uploaded Successfully</p>");
                        ViewBag.data = data.ToArray();
                        return View();
                    }
                    else
                    {
                        //alert message for invalid file format
                        data.Add("<ul>");
                        data.Add("<li>Only Excel File Format is Allowed</li>");
                        data.Add("</ul>");
                        data.ToArray();
                        return Json(data, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    data.Add("<ul>");
                    if (FileUpload == null) data.Add("<li>Please Choose Excel File</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }

            }
            catch (SystemException sysException)
            {

                Console.WriteLine(sysException.Message);
                data.Add("<ul>");
                data.Add("<li>" + sysException.Message + " " + sysException.InnerException + "</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception x)
            {

                data.Add("<ul>");
                data.Add("<li>" + x.Message + " " + x.InnerException + "</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }

        }

        // GET: Attatchments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attatchment attatchment = db.Attatchments.Include(a => a.traine).Include(a => a.activity).Where(a => a.Id == id).ToList().FirstOrDefault();
            return View(attatchment);
        }

        // GET: Attatchments/Create
        public ActionResult Create()
        {
            ViewBag.activityId = new SelectList(db.Activities, "ActivityId", "ActivityName");
            ViewBag.traineId = new SelectList(db.Users.Where(a=>a.userType == ApplicationUser.UserType.trainee), "Id", "firstName");
            return View();
        }

        // POST: Attatchments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Attatchment attatchment)
        {
            if (ModelState.IsValid)
            {
                db.Attatchments.Add(attatchment);
                db.SaveChanges();
                return RedirectToAction("Index", new { activityId = attatchment.activityId, type = attatchment.type });
            }

            ViewBag.activityId = new SelectList(db.Activities.Where(a => a.ActivityId == attatchment.activityId), "ActivityId", "ActivityName", attatchment.activityId);
            ViewBag.traineId = new SelectList(db.Users.Where(u => u.Id == attatchment.traineId), "Id", "firstName", attatchment.traineId);
            return View(attatchment);
        }

        // GET: Attatchments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attatchment attatchment = db.Attatchments.Include(a => a.traine).Include(a => a.activity).Where(a => a.Id == id).ToList().FirstOrDefault();
            if (attatchment == null)
            {
                return HttpNotFound();
            }
            ViewBag.activityId = new SelectList(db.Activities.Where(a=>a.ActivityId == attatchment.activityId), "ActivityId", "ActivityName", attatchment.activityId);
            ViewBag.traineId = new SelectList(db.Users.Where(u => u.Id == attatchment.traineId), "Id", "firstName", attatchment.traineId);
            return View(attatchment);
        }

        // POST: Attatchments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Attatchment attatchment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attatchment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index" , new { activityId = attatchment.activityId , type = attatchment.type });
            }
            ViewBag.activityId = new SelectList(db.Activities.Where(a => a.ActivityId == attatchment.activityId), "ActivityId", "ActivityName", attatchment.activityId);
            ViewBag.traineId = new SelectList(db.Users.Where(u=>u.Id == attatchment.traineId), "Id", "firstName", attatchment.traineId);
            return View(attatchment);
        }

        // GET: Attatchments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attatchment attatchment = db.Attatchments.Include(a => a.traine).Include(a => a.activity).Where(a => a.Id == id).ToList().FirstOrDefault();
            if (attatchment == null)
            {
                return HttpNotFound();
            }
            return View(attatchment);
        }

        // POST: Attatchments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Attatchment attatchment = db.Attatchments.Include(a=>a.traine).Include(a => a.activity).Where(a => a.Id == id).ToList().FirstOrDefault();
            db.Attatchments.Remove(attatchment);
            db.SaveChanges();
            return RedirectToAction("Index", new { activityId = attatchment.activityId, type = attatchment.type });
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
