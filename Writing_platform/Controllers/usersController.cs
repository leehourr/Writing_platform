using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using Writing_platform.Models;


namespace Writing_platform.Controllers
{
    public class usersController : Controller
    {
        private WritingPlatformEntities db = new WritingPlatformEntities();
        public Authenthication auth = new Authenthication();
        public AuthDBContext authDB = new AuthDBContext();
        private int uid;
        private int Id
        {
            get { return uid; }
            set { uid = value; }
        }

        public user user_list = new user();
        protected int status_code = 200;
        protected string message = "Success";

        // GET: users
        [Route("works/{id}")]

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string email = form["email"];
            string password = form["password"];
            bool emailFound = false;
            string encryptedPassword = auth.GetSHA256Hash(password);
            //validate the credential
            foreach (user u in db.users)
            {
                if (u.email == email && u.password == encryptedPassword)
                {
                    Id = u.id;
                    if (u.status == "deleted")
                    {
                        status_code = 401;
                        message = "No account found with that email address. Try a different one or create a new account";
                        return Json(new { statCode = status_code, message = message, uid = u.id }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new { statCode = status_code, message = message, uid= u.id,id= uid}, JsonRequestBehavior.AllowGet);
                }

                if (u.email == email && u.password != encryptedPassword)
                {
                    emailFound = true;
                    status_code = 401;
                    message = "Password is incorrect.";
                }
            }

            if (!emailFound)
            {
                status_code = 401;
                message = "No account found with that email address. Try a different one or create a new account";
            }

            return Json(new { statCode = status_code, message = message}, JsonRequestBehavior.AllowGet);
        }

        // GET: users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            user user = db.users.OrderByDescending(u => u.id).FirstOrDefault();
            string name = form["name"];
            string email = form["email"];
            string password = form["password"];

            foreach (user u in db.users)
            {
                if (u.email == email)
                {
                    status_code = 401;
                    message = $"{email} is already existed.";
                    return Json(new { statCode = status_code, message = message }, JsonRequestBehavior.AllowGet);
                }
            }

            string encryptedPassword = auth.GetSHA256Hash(password);

            user_list.id = user.id + 1;
            user_list.email = email;
            user_list.name = name;
            user_list.password = encryptedPassword;
            db.users.Add(user_list);
            db.SaveChanges();
            return Json(new { statCode = 200, message = message }, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public JsonResult Update(int id, string name, string email, string pass)
        {
            user user = db.users.Find(id);
            string uName = name;
            string uEmail = email;
            string password = pass;
            
            foreach (user u in db.users)
            {
                if(u.id != user.id && u.email == uEmail)
                {
                    return Json(new {email, status_code = 401 , message = "This email address already existed." }, JsonRequestBehavior.AllowGet);
                }
            }

            string encryptedPassword = auth.GetSHA256Hash(password);

            user.name = uName;
            user.email = uEmail;
            user.password = encryptedPassword;
            user.status = "updated";

            db.users.AddOrUpdate(user);
            db.SaveChanges();
            return Json(new {status_code = status_code, message = message, email}, JsonRequestBehavior.AllowGet);
        }

        // GET: users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,email,password")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            return Json(new { redirect = "users/Login" }, JsonRequestBehavior.AllowGet);
        }
        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
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
