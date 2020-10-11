using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GraduationProject.Models;
using Microsoft.AspNet.Identity;

namespace GraduationProject.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Profiles
        [Authorize(Roles ="Admins")]
        public ActionResult Index(string type)
        {
            ViewBag.Type = type;
            if (type == null)
                return View(db.Profiles.ToList());
            else if (type == "Applicants")
                return View(db.Profiles.Where(p => p.user.UserType == "Applicants").ToList());
            else if(type == "Publishers")
                return View(db.Profiles.Where(p => p.user.UserType == "Publishers").ToList());
            else
                return HttpNotFound();
        }
        
        // GET: Profiles/Details/5
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
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }
        
        // GET: Profiles/Edit/5
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
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.Military = new SelectList(new[] { "Not Applicable", "Exempted", "Completed", "Postponed" }, profile.Military);
            ViewBag.State = new SelectList(new[] { "Unspecified", "Single", "Married" }, profile.State);
            ViewBag.Gender = new SelectList(new[] { "Male", "Female" }, profile.Gender);
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Profile profile, HttpPostedFileBase CV, HttpPostedFileBase img)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    if (profile.img != null)
                    {
                        string oldpath = Path.Combine(Server.MapPath("~/Uploads/Profile"), profile.img);
                        System.IO.File.Delete(oldpath);
                    }
                    string path = Path.Combine(Server.MapPath("~/Uploads/Profile"), img.FileName);
                    img.SaveAs(path);
                    profile.img = img.FileName;
                }
                
                if (CV != null)
                {
                    if (profile.CV != null)
                    {
                        string oldpath = Path.Combine(Server.MapPath("~/Uploads/CV"), profile.CV);
                        System.IO.File.Delete(oldpath);
                    }
                    string path = Path.Combine(Server.MapPath("~/Uploads/CV"), CV.FileName);
                    CV.SaveAs(path);
                    profile.CV = CV.FileName;
                }

                profile.UserId = User.Identity.GetUserId();
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new { profile.id });
            }

            ViewBag.Military = new SelectList(new[] { "Not Applicable", "Exempted", "Completed", "Postponed" }, profile.Military);
            ViewBag.State = new SelectList(new[] { "Unspecified", "Single", "Married" }, profile.State);
            ViewBag.Gender = new SelectList(new[] { "Male", "Female" }, profile.Gender);
            return View(profile);
        }

        // GET: Profiles/Delete/5
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
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var us = User.Identity.GetUserId();
            if (db.Profiles.Count(p => p.UserId == us) == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            Profile profile = db.Profiles.Find(id);

            if (profile.CV != null)
            {
                string oldpath = Path.Combine(Server.MapPath("~/Uploads/CV"), profile.CV);
                System.IO.File.Delete(oldpath);
            }
            if (profile.img != null)
            {
                string oldpath = Path.Combine(Server.MapPath("~/Uploads/Profile"), profile.img);
                System.IO.File.Delete(oldpath);
            }
            if (profile.UserId == User.Identity.GetUserId())
            {
                if (profile.user.UserType == "Publishers")
                    foreach (var item in db.Jobs.Where(j => j.UserId == profile.UserId))
                    {
                        if (item.JobImage != null)
                        {
                            string oldpath = Path.Combine(Server.MapPath("~/Uploads"), item.JobImage);
                            System.IO.File.Delete(oldpath);
                        }
                        db.Jobs.Remove(item);
                    }
                if (profile.user.UserType == "Applicants")
                    foreach (var item in db.ApplyForJobs.Where(j => j.UserId == profile.UserId))
                    {
                        db.ApplyForJobs.Remove(item);
                    }
                db.Profiles.Remove(profile);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            else if(profile.user.UserType == "Admins")
            {
                db.Profiles.Remove(profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index", "Home");
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
