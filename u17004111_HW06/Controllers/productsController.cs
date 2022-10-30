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
    public class productsController : Controller
    {
        private BikeStoresDb db = new BikeStoresDb();

        // GET: products
        public ActionResult Index(int? page, string namesearch, string CurrentFilterTextbox)
        {
            IQueryable<product> products = db.products.Include(p => p.brand).Include(p => p.category).OrderBy(product => product.product_id);

            if (namesearch != null)
            {
                page = 1;
            }
            else
            {
                namesearch = CurrentFilterTextbox;
            }

            ViewBag.CurrentFilterTextbox = namesearch;

            var nametemp = products;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagelistedprod = products.ToPagedList(pageNumber, pageSize);

            if (!String.IsNullOrEmpty(namesearch))
            {
                nametemp = nametemp.Where(xx => xx.product_name.Contains(namesearch));
                pagelistedprod = nametemp.ToPagedList(pageNumber, pageSize);
            }
            else
            {
                pagelistedprod = products.ToPagedList(pageNumber, pageSize);
            }           

            return View(pagelistedprod);
        }

        public ActionResult Details(int id)
        {
            //db.Configuration.ProxyCreationEnabled = false;
            product prod = db.products.FirstOrDefault(x => x.product_id == id);
            
            //specify the name or path of the partial view
            return PartialView("Details_Partial", prod);
        }

        // GET: products/Create
        public ActionResult Create()
        {
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name");
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            return PartialView("Create_Partial");
        }

        // POST: products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = await db.products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);

            return PartialView("Edit_Partial", product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "product_id,product_name,brand_id,category_id,model_year,list_price")] product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.brand_id = new SelectList(db.brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = await db.products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete_Partial", product);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            product product = await db.products.FindAsync(id);
            db.products.Remove(product);
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