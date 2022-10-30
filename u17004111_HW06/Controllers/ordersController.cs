using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using u17004111_HW06.Models;
using PagedList;

namespace u17004111_HW06.Views.Home
{
    public class ordersController : Controller
    {
        private BikeStoresDb db = new BikeStoresDb();

        // GET: orders
        public ActionResult Index(int? page, string datesearch, string CurrentFilterTextbox)
        {
            IQueryable<order> orders = db.orders.OrderBy(order => order.order_id);

            if (datesearch != null)
            {
                page = 1;
            }
            else
            {
                datesearch = CurrentFilterTextbox;
            }

            ViewBag.CurrentFilterTextbox = datesearch;

            var ordertemp = orders;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagelistedprod = orders.ToPagedList(pageNumber, pageSize);

            if (!String.IsNullOrEmpty(datesearch))
            {
                ordertemp = ordertemp.Where(xx => xx.order_date.ToString().Contains(datesearch));
                pagelistedprod = ordertemp.ToPagedList(pageNumber, pageSize);
            }
            else
            {
                pagelistedprod = orders.ToPagedList(pageNumber, pageSize);
            }

            return View(pagelistedprod);
        }

        // GET: orders/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = await db.orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: orders/Create
        public ActionResult Create()
        {
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name");
            ViewBag.staff_id = new SelectList(db.staffs, "staff_id", "first_name");
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name");
            return View();
        }

        // POST: orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "order_id,customer_id,order_status,order_date,required_date,shipped_date,store_id,staff_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.orders.Add(order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", order.customer_id);
            ViewBag.staff_id = new SelectList(db.staffs, "staff_id", "first_name", order.staff_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", order.store_id);
            return View(order);
        }

        // GET: orders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = await db.orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", order.customer_id);
            ViewBag.staff_id = new SelectList(db.staffs, "staff_id", "first_name", order.staff_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", order.store_id);
            return View(order);
        }

        // POST: orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "order_id,customer_id,order_status,order_date,required_date,shipped_date,store_id,staff_id")] order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.customers, "customer_id", "first_name", order.customer_id);
            ViewBag.staff_id = new SelectList(db.staffs, "staff_id", "first_name", order.staff_id);
            ViewBag.store_id = new SelectList(db.stores, "store_id", "store_name", order.store_id);
            return View(order);
        }

        // GET: orders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = await db.orders.FindAsync(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            order order = await db.orders.FindAsync(id);
            db.orders.Remove(order);
            await db.SaveChangesAsync();
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
