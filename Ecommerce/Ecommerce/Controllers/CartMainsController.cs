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
    public class CartMainsController : Controller
    {
        private ProductDBContext db = new ProductDBContext();

        // GET: CartMains
        public ActionResult Index()
        {
            var cartMains = db.CartMains.Include(c => c.DeliveryDetails);
            return View(cartMains.ToList());
        }

        // GET: CartMains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartMain cartMain = db.CartMains.Find(id);
            if (cartMain == null)
            {
                return HttpNotFound();
            }
            return View(cartMain);
        }

        // GET: CartMains/Create
        public ActionResult Create()
        {
            ViewBag.delivery_id = new SelectList(db.DeliveryDetails, "Id", "billing_first_name");
            return View();
        }

        // POST: CartMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cart_id,cart_date,user_id,total,Discount,delivery_id")] CartMain cartMain)
        {
            if (ModelState.IsValid)
            {
                db.CartMains.Add(cartMain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.delivery_id = new SelectList(db.DeliveryDetails, "Id", "billing_first_name", cartMain.delivery_id);
            return View(cartMain);
        }

        // GET: CartMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartMain cartMain = db.CartMains.Find(id);
            if (cartMain == null)
            {
                return HttpNotFound();
            }
            ViewBag.delivery_id = new SelectList(db.DeliveryDetails, "Id", "billing_first_name", cartMain.delivery_id);
            return View(cartMain);
        }

        // POST: CartMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cart_id,cart_date,user_id,total,Discount,delivery_id")] CartMain cartMain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartMain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.delivery_id = new SelectList(db.DeliveryDetails, "Id", "billing_first_name", cartMain.delivery_id);
            return View(cartMain);
        }

        // GET: CartMains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartMain cartMain = db.CartMains.Find(id);
            if (cartMain == null)
            {
                return HttpNotFound();
            }
            return View(cartMain);
        }

        // POST: CartMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartMain cartMain = db.CartMains.Find(id);
            db.CartMains.Remove(cartMain);
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
