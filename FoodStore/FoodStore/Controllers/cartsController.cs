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
    public class cartsController : Controller
    {
        private storeEntities db = new storeEntities();


        [HttpPost]
        public ActionResult IncreaseQuantity(int cartItemId)
        {
            var correctid = cartItemId - 1;
            var cartItem = db.cart_item.FirstOrDefault(ci => ci.id == correctid);
            if (cartItem != null)
            {
                cartItem.quantity++;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DecreaseQuantity(int cartItemId)
        {
            var correctid = cartItemId + 1;
            var cartItem = db.cart_item.FirstOrDefault(ci => ci.id == correctid);
            if (cartItem != null && cartItem.quantity > 1)
            {
                cartItem.quantity--;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteItem(int cartItemId)
        {
            var cartItem = db.cart_item.FirstOrDefault(ci => ci.id == cartItemId);
            if (cartItem != null)
            {
                db.cart_item.Remove(cartItem);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: carts
        public ActionResult Index()
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            var carts = db.carts.Include(c => c.user).Include(c => c.cart_item.Select(ci => ci.product));
            return View(carts.ToList());
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            if (Session["id"] == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                int userId = (int)Session["id"];
                cart cart = db.carts.SingleOrDefault(c => c.user_id == userId);
                if (cart == null)
                {

                    cart = new cart
                    {
                        user_id = userId
                    };
                    db.carts.Add(cart);
                    db.SaveChanges();
                }

                var cartItem = db.cart_item.SingleOrDefault(ci => ci.cart_id == cart.id && ci.product_id == productId);

                if (cartItem == null)
                {
                    cartItem = new cart_item
                    {
                        product_id = productId,
                        quantity = quantity,
                        cart_id = cart.id
                    };
                    db.cart_item.Add(cartItem);
                }
                else
                {
                    cartItem.quantity += quantity;
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }
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
