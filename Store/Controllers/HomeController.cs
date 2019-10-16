using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Store.Models;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        StoreContext db = new StoreContext();

        [HttpGet]
        public ActionResult Index(int? id,string sort)
        {
            if (id == null)
            {
                IEnumerable<Product> products = db.Products.OrderByDescending(i => i.Id);
                ViewBag.HeaderText = "Недавно добавленные";
                switch (sort)
                {
                    case "price_asc":
                        products = products.OrderBy(i => i.Price);
                        ViewBag.HeaderText = "Сортировка: Цена по возрастанию";
                        break;
                    case "price_desc":
                        products = products.OrderByDescending(i => i.Price);
                        ViewBag.HeaderText = "Сортировка: Цена по убыванию";
                        break;
                    case "newest":
                        products = products.OrderByDescending(i => i.Id);
                        ViewBag.HeaderText = "Сортировка: Сначала новые";
                        break;
                    case "oldest":
                        products = products.OrderBy(i => i.Id);
                        ViewBag.HeaderText = "Сортировка: Сначала старые";
                        break;
                }
                return View(products.ToList());
            }
            else
            {
                IEnumerable<Product> products = db.Products
                    .Where(i => i.CategoryId == id)
                    .OrderByDescending(p => p.Id);
                ViewBag.HeaderText = db.Categories.Find(id).CategoryName;
                switch (sort)
                {
                    case "price_asc":
                        products = products.OrderBy(i => i.Price);
                        ViewBag.HeaderText += ", Сортировка: Цена по возрастанию";
                        break;
                    case "price_desc":
                        products = products.OrderByDescending(i => i.Price);
                        ViewBag.HeaderText += ", Сортировка: Цена по убыванию";
                        break;
                    case "newest":
                        products = products.OrderByDescending(i => i.Id);
                        ViewBag.HeaderText += ", Сортировка: Сначала новые";
                        break;
                    case "oldest":
                        products = products.OrderBy(i => i.Id);
                        ViewBag.HeaderText += ", Сортировка: Сначала старые";
                        break;
                }
                return View(products.ToList());
            }
        }

        [HttpGet]
        public ActionResult Search (string keyword)
        {
            if (String.IsNullOrEmpty(keyword)) return HttpNotFound();
            var products = db.Products.Where(i => i.Category.CategoryName.Contains(keyword) || i.Brand.BrandName.Contains(keyword)
            || i.Model.Contains(keyword) || i.Series.Contains(keyword));
            if (!products.Any()) return View("NotFound");
            ViewBag.HeaderText = "По вашему запросу " + keyword + " было найдено:";
            return View("Index", products.ToList());
        }

        [HttpGet]
        public ActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ContactUs()
        {
            return View();
        }
    }
}