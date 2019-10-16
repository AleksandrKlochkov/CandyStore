using Store.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class BrandController : Controller
    {
        
        StoreContext db = new StoreContext();
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Add([Bind(Include = "BrandName")]Brand brand, bool exit)
        {
            if (!exit)
            {
                var dublicateBrand = db.Brands.Where(i => i.BrandName.Equals(brand.BrandName));
                if (dublicateBrand.Any())
                {
                    ModelState.AddModelError("", "Таблица Брэндов уже содержит " + brand.BrandName);
                    return View(brand);
                }
                else
                {
                    db.Brands.Add(brand);
                    db.SaveChanges();
                    return RedirectToRoute(new { controller = "Brand", action = "BrandList" });
                }  
            }
            else
            {
                var dublicateBrand = db.Brands.Where(i => i.BrandName.Equals(brand.BrandName));
                if (dublicateBrand.Any())
                {
                    ModelState.AddModelError("", "Таблица Брэндов уже содержит " + brand.BrandName);
                    return View(brand);
                }
                else
                {
                    db.Brands.Add(brand);
                    db.SaveChanges();
                    return RedirectToRoute(new { controller = "Brand", action = "Add" });
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult BrandList()
        {
            IEnumerable<Brand> brands = db.Brands.OrderBy(i => i.BrandName);
            return View(brands.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand == null) return HttpNotFound();
            else
            {
                db.Entry(brand).State = EntityState.Deleted;
                db.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Brand", action = "BrandList" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Brand brand = db.Brands.Find(id);
            if (brand != null)
            {
                return View(brand);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Brand brand)
        {
            db.Entry(brand).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToRoute(new { controller = "Brand", action = "BrandList" });
        }
    }
}