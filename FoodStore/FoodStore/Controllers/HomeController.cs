using FoodStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FoodStore.Controllers
{
    public class HomeController : Controller
    {
        private storeEntities db = new storeEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Regester()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Regester([Bind(Include = "id,name,email,password,type,image_url")] user user, string confirmpassword)
        {
            if (user.password != confirmpassword)
            {
                ModelState.AddModelError("confirmpassword", "The password and confirmation password do not match.");
            }

            if (ModelState.IsValid)
            {
                db.users.Add(user);
                cart cart = new cart()
                {
                    user_id = user.id,
                };
                db.carts.Add(cart);
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            var user = db.users.SingleOrDefault(u => u.email == Email);
            if (user != null && user.password == Password)
            {
                Session["id"] = user.id;
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ErrorMessage = "Invalid Username or password.";
            return View();
        }

        public ActionResult Logout()
        {
            Session["id"] = null;
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Profile()
        {
            if(Session["id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var id = Session["id"];
            var user = db.users.Find(id);
            return View(user);

        }

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
        public ActionResult Edit([Bind(Include = "id,name,email,password,type,image_url")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }
            return View(user);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}