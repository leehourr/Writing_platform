using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Writing_platform.Models;

namespace Writing_platform.Controllers
{
    public class ratesController : Controller
    {
        private WritingPlatformEntities db = new WritingPlatformEntities();

        // GET: rates
        public ActionResult Index()
        {
            var rates = db.rates.Include(r => r.user).Include(r => r.work);
            return View(rates.ToList());
        }

        // GET: rates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rate rate = db.rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // GET: rates/Create
        public ActionResult Create()
        {
            ViewBag.uid = new SelectList(db.users, "id", "name");
            ViewBag.work_id = new SelectList(db.works, "id", "title");
            return View();
        }

        // POST: rates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,uid,work_id,rating")] rate rate)
        {
            if (ModelState.IsValid)
            {
                db.rates.Add(rate);
                db.SaveChanges();
                return View(rate);
            }

            ViewBag.uid = new SelectList(db.users, "id", "name", rate.uid);
            ViewBag.work_id = new SelectList(db.works, "id", "title", rate.work_id);
            return View(rate);
        }

        // GET: rates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rate rate = db.rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            ViewBag.uid = new SelectList(db.users, "id", "name", rate.uid);
            ViewBag.work_id = new SelectList(db.works, "id", "title", rate.work_id);
            return View(rate);
        }

        // POST: rates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,uid,work_id,rating")] rate rate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.uid = new SelectList(db.users, "id", "name", rate.uid);
            ViewBag.work_id = new SelectList(db.works, "id", "title", rate.work_id);
            return View(rate);
        }

        // GET: rates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            rate rate = db.rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // POST: rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            rate rate = db.rates.Find(id);
            db.rates.Remove(rate);
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
