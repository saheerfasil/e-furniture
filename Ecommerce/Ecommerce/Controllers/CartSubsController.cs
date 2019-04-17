using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ecommerce.Models;

namespace Ecommerce.Controllers
{
    public class CartSubsController : Controller
    {
        private ProductDBContext db = new ProductDBContext();

        // GET: CartSubs
        public ActionResult Index()
        {
            var cartSubs = db.CartSubs.Include(c => c.CartMain).Include(c=>c.CartMain.DeliveryDetails).Include(c => c.Product);
            return View(cartSubs.ToList());
        }

        // GET: CartSubs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartSub cartSub = db.CartSubs.Find(id);
            if (cartSub == null)
            {
                return HttpNotFound();
            }
            return View(cartSub);
        }

        // GET: CartSubs/Create
        public ActionResult Create()
        {
            ViewBag.cart_id = new SelectList(db.CartMains, "cart_id", "user_id");
            ViewBag.pro_id = new SelectList(db.Products, "id", "name");
            return View();
        }

        // POST: CartSubs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "subcr_id,cart_id,pro_id,quantity,type,color,total")] CartSub cartSub)
        {
            if (ModelState.IsValid)
            {
                db.CartSubs.Add(cartSub);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cart_id = new SelectList(db.CartMains, "cart_id", "user_id", cartSub.cart_id);
            ViewBag.pro_id = new SelectList(db.Products, "id", "name", cartSub.pro_id);
            return View(cartSub);
        }

        // GET: CartSubs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartSub cartSub = db.CartSubs.Find(id);
            if (cartSub == null)
            {
                return HttpNotFound();
            }
            ViewBag.cart_id = new SelectList(db.CartMains, "cart_id", "user_id", cartSub.cart_id);
            ViewBag.pro_id = new SelectList(db.Products, "id", "name", cartSub.pro_id);
            return View(cartSub);
        }

        // POST: CartSubs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "subcr_id,cart_id,pro_id,quantity,type,color,total")] CartSub cartSub)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartSub).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cart_id = new SelectList(db.CartMains, "cart_id", "user_id", cartSub.cart_id);
            ViewBag.pro_id = new SelectList(db.Products, "id", "name", cartSub.pro_id);
            return View(cartSub);
        }

        // GET: CartSubs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartSub cartSub = db.CartSubs.Find(id);
            if (cartSub == null)
            {
                return HttpNotFound();
            }
            return View(cartSub);
        }

        // POST: CartSubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartSub cartSub = db.CartSubs.Find(id);
            db.CartSubs.Remove(cartSub);
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
