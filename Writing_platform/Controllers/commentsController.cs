using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Writing_platform.Models;

namespace Writing_platform.Controllers
{
    public class commentsController : Controller
    {
        private WritingPlatformEntities db = new WritingPlatformEntities();
        protected comment cmt_list = new comment();
        // GET: comments
        public ActionResult Index()
        {
            var comments = db.comments.Include(c => c.work).Include(c => c.user);
            return View(comments.ToList());
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
 
        // GET: comments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        public ActionResult Upload(int wid, int id, string cmt)
        {
            comment comment = db.comments.OrderByDescending(c => c.id).FirstOrDefault();
            if(comment == null)
            {
                cmt_list.id = 1;
            }
            else
            {
                cmt_list.id = comment.id + 1;
            }
            cmt_list.uid = id;
            cmt_list.u_comment = cmt;
            cmt_list.work_id = wid;
            db.comments.Add(cmt_list);
            db.SaveChanges();
            return Json(new { statCode = 200,wid,id, cmt }, JsonRequestBehavior.AllowGet);
        }

        // GET: comments/Create


        // POST: comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,uid,work_id,comment1")] comment comment)
        {
            if (ModelState.IsValid)
            {
                db.comments.Add(comment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.work_id = new SelectList(db.works, "id", "title", comment.work_id);
            ViewBag.uid = new SelectList(db.users, "id", "name", comment.uid);
            return View(comment);
        }

        // GET: comments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.work_id = new SelectList(db.works, "id", "title", comment.work_id);
            ViewBag.uid = new SelectList(db.users, "id", "name", comment.uid);
            return View(comment);
        }

        // POST: comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,uid,work_id,comment1")] comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.work_id = new SelectList(db.works, "id", "title", comment.work_id);
            ViewBag.uid = new SelectList(db.users, "id", "name", comment.uid);
            return View(comment);
        }

        // GET: comments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }

        // POST: comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            comment comment = db.comments.Find(id);
            db.comments.Remove(comment);
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
