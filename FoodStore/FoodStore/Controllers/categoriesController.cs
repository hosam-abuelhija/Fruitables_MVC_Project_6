using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FoodStore.Models;

namespace FoodStore.Controllers
{
    public class categoriesController : Controller
    {
        private storeEntities db = new storeEntities();

        // GET: categories
        public ActionResult Index()
        {
            return View(db.categories.ToList());
        }

        public ActionResult Fruits()
        {
            return View(db.products.ToList());
        }

        public ActionResult Vegetables()
        {
            return View(db.products.ToList());
        }

        public ActionResult Juices()
        {
            return View(db.products.ToList());
        }

        public ActionResult Product(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = db.products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
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
