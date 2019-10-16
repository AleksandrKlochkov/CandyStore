using Store.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.Controllers
{
    public class CharacteristicController : Controller
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
        public ActionResult Add([Bind(Include = "CharacName,Unit")]Characteristic characteristic, bool exit)
        {
            if (!exit)
            {
                db.Characteristics.Add(characteristic);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "Characteristic", action = "CharList" });
            }
            else
            {
                db.Characteristics.Add(characteristic);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "Characteristic", action = "Add" });
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult CharList()
        {
            IEnumerable<Characteristic> characteristics = db.Characteristics.OrderBy(i => i.CharacName);
            return View(characteristics.ToList());
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            Characteristic characteristic = db.Characteristics.Find(id);
            if (characteristic == null) return HttpNotFound();
            else
            {
                db.Entry(characteristic).State = EntityState.Deleted;
                db.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Characteristic", action = "CharList" });
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
            Characteristic characteristic = db.Characteristics.Find(id);
            if (characteristic != null)
            {
                return View(characteristic);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Edit(Characteristic characteristic)
        {
            db.Entry(characteristic).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToRoute(new { controller = "Characteristic", action = "CharList" });
        }
    }
}