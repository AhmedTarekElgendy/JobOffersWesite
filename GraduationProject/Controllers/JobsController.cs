using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GraduationProject.Models;
using System.IO;
using Microsoft.AspNet.Identity;

namespace GraduationProject.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Jobs/
        public ActionResult Index()
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index","Home");
            }
            //var jobs = db.Jobs.Include(j => j.Category);
            //return View(jobs.ToList());
            return View(db.Categories.ToList());
        }
        public ActionResult PublisherJobs()
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            //var jobs = db.Jobs.Include(j => j.Category);
            //return View(jobs.ToList());
            return View(db.Categories.ToList());
        }
        // GET: /Jobs/Details/5
        public ActionResult Details(int? id)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = id;
            return View(job);
        }

        // GET: /Jobs/Create
        public ActionResult Create()
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "CategoryName");
            ViewBag.Military = new SelectList(new[] { "All","Not Applicable", "Exempted", "Completed", "Postponed" }, "All");
            ViewBag.State = new SelectList(new[] { "All", "Unspecified", "Single", "Married" }, "All");
            ViewBag.Gender = new SelectList(new[] { "All", "Male", "Female" }, "All");
            return View();
        }

        // POST: /Jobs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Job job,HttpPostedFileBase upload)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                upload.SaveAs(path);
                job.JobImage = upload.FileName;
                job.UserId = User.Identity.GetUserId();
                db.Jobs.Add(job);
                db.SaveChanges();
                return RedirectToAction("PublisherJobs");
            }

            ViewBag.CategoryId = new SelectList(db.Categories, "id", "CategoryName");
            ViewBag.Military = new SelectList(new[] { "All", "Not Applicable", "Exempted", "Completed", "Postponed" }, "All");
            ViewBag.State = new SelectList(new[] { "All", "Unspecified", "Single", "Married" }, "All");
            ViewBag.Gender = new SelectList(new[] { "All", "Male", "Female" }, "All");
            return View(job);
        }

        // GET: /Jobs/Edit/5
        public ActionResult Edit(int? id)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "CategoryName", job.Category.CategoryName);
            ViewBag.Military = new SelectList(new[] { "Not Applicable", "Exempted", "Completed", "Postponed" }, job.Military);
            ViewBag.State = new SelectList(new[] { "Unspecified", "Single", "Married" }, job.State);
            ViewBag.Gender = new SelectList(new[] { "Male", "Female" }, job.Gender);
            return View(job);
        }

        // POST: /Jobs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Job job, HttpPostedFileBase upload)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                string oldpath = Path.Combine(Server.MapPath("~/Uploads"), job.JobImage);
                if (upload != null)
                {
                    System.IO.File.Delete(oldpath);
                    string path = Path.Combine(Server.MapPath("~/Uploads"), upload.FileName);
                    upload.SaveAs(path);
                    job.JobImage = upload.FileName;
                }

                job.UserId = User.Identity.GetUserId();
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("PublisherJobs");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "id", "CategoryName", job.Category.CategoryName);
            ViewBag.Military = new SelectList(new[] { "Not Applicable", "Exempted", "Completed", "Postponed" }, job.Military);
            ViewBag.State = new SelectList(new[] { "Unspecified", "Single", "Married" }, job.State);
            ViewBag.Gender = new SelectList(new[] { "Male", "Female" }, job.Gender);
            return View(job);
        }

        // GET: /Jobs/Delete/5
        public ActionResult Delete(int? id)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Job job = db.Jobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: /Jobs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            Job job = db.Jobs.Find(id);
            string oldpath = Path.Combine(Server.MapPath("~/Uploads"), job.JobImage);
            System.IO.File.Delete(oldpath);
            db.Jobs.Remove(job);
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
