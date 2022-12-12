using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Writing_platform.Models;


namespace Writing_platform.Controllers
{

    public class worksController : Controller
    {
        private WritingPlatformEntities db = new WritingPlatformEntities();
        public Authenthication auth = new Authenthication();
        public AuthDBContext authDBContext = new AuthDBContext();
        public work work_list = new work();

        protected int uid = 0;   
        // GET: works
        [Route("works/{id}")]
        public ActionResult Index(int? id)
        {
            auth.Id = (int)id;
            uid = (int)id;
            ViewBag.Id=auth.Id;
            user user = db.users.Find(id);
            work work = db.works.FirstOrDefault();
            ViewBag.name = user.name;
            ViewBag.email = user.email;
            ViewBag.pass = user.password;
            var works = db.works.Include(w => w.user).OrderByDescending(w=>w.id);
            return View(works.ToList());
        }

        public ActionResult DeleteAccount(int? id)
        {
            user user = db.users.Find(id);
            user.status = "deleted";
            user.email = null;
            user.password = null;
            db.users.AddOrUpdate(user);
            db.SaveChanges();
            return Json(new {statCode =301, redirect = "users/Login"},JsonRequestBehavior.AllowGet);
        }

        public ActionResult Upload(int  uid,string username, string title , string genre, string story, DateTime dop)
        {
            work work = db.works.OrderByDescending(w => w.id).FirstOrDefault();
            work_list.id = work.id + 1;
            work_list.uid = uid;
            work_list.title = title;
            work_list.genre = genre;
            work_list.story = story;
            work_list.dop = dop;
            work_list.user_name = username;
            db.works.Add(work_list);
            db.SaveChanges();
            return Json(new { statCode = 200, name= work_list.user_name, work= work_list.story },JsonRequestBehavior.AllowGet);
        }

 
        // GET: works/Details/5
        public ActionResult Details(int? id, int? story_id, string name)
        {
            var content = new contentDetail();
            ViewBag.name = name;
            ViewBag.uid = id;
            ViewBag.work_id = story_id;
            int work_id = (int)story_id;
            work work = db.works.Find(story_id);
            var uname = new List<string>();

            foreach (var c in db.comments.Where(c => c.work_id == work_id))
            {
                content.comment+=","+ c.u_comment;
                foreach(user u in db.users.Where(x => x.id == c.uid))
                {
                    content.user +="," +u.name;
                }
            }
            content.work = db.works.Find(story_id);

            return View(content);
        }

        // GET: works/Create
        public ActionResult Create()
        {
            ViewBag.uid = new SelectList(db.users, "id", "name");
            return View();
        }

        // POST: works/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,uid,title,genre,dop,work1")] work work)
        {
            if (ModelState.IsValid)
            {
                db.works.Add(work);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.uid = new SelectList(db.users, "id", "name", work.uid);
            return View(work);
        }

        // GET: works/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            work work = db.works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            ViewBag.uid = new SelectList(db.users, "id", "name", work.uid);
            return View(work);
        }

        // POST: works/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,uid,title,genre,dop,work1")] work work)
        {
            if (ModelState.IsValid)
            {
                db.Entry(work).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.uid = new SelectList(db.users, "id", "name", work.uid);
            return View(work);
        }

        // GET: works/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            work work = db.works.Find(id);
            if (work == null)
            {
                return HttpNotFound();
            }
            return View(work);
        }

        // POST: works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            work work = db.works.Find(id);
            db.works.Remove(work);
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
