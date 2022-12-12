using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Writing_platform.Models;

namespace Writing_platform.Views
{
    public class WritingPlatformController : Controller
    {
        private WritingPlatformEntities db = new WritingPlatformEntities();

        // GET: WritingPlatform
        public ActionResult Index()
        {
            return View(db.sysdiagrams.ToList());
        }

        // GET: WritingPlatform/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sysdiagram sysdiagram = db.sysdiagrams.Find(id);
            if (sysdiagram == null)
            {
                return HttpNotFound();
            }
            return View(sysdiagram);
        }

        // GET: WritingPlatform/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WritingPlatform/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "name,principal_id,diagram_id,version,definition")] sysdiagram sysdiagram)
        {
            if (ModelState.IsValid)
            {
                db.sysdiagrams.Add(sysdiagram);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sysdiagram);
        }

        // GET: WritingPlatform/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sysdiagram sysdiagram = db.sysdiagrams.Find(id);
            if (sysdiagram == null)
            {
                return HttpNotFound();
            }
            return View(sysdiagram);
        }

        // POST: WritingPlatform/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "name,principal_id,diagram_id,version,definition")] sysdiagram sysdiagram)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sysdiagram).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sysdiagram);
        }

        // GET: WritingPlatform/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sysdiagram sysdiagram = db.sysdiagrams.Find(id);
            if (sysdiagram == null)
            {
                return HttpNotFound();
            }
            return View(sysdiagram);
        }

        // POST: WritingPlatform/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            sysdiagram sysdiagram = db.sysdiagrams.Find(id);
            db.sysdiagrams.Remove(sysdiagram);
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
