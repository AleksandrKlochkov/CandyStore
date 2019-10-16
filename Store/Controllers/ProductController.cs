using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Store.Models;

namespace Store.Controllers
{
    public class ProductController : Controller
    {
        StoreContext db = new StoreContext();

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Add()
        {
            ViewBag.Title = "Добавление продукта";
            PopulateBrandsDropDownList();
            PopulateCategoriesDropDownList();
            return View();
        }

        private void PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var categoriesQuery = from c in db.Categories
                                  orderby c.Id
                                  select c;
            ViewBag.CategoryId = new SelectList(categoriesQuery, "Id", "CategoryName", selectedCategory);
        }

        private void PopulateBrandsDropDownList(object selectedBrand = null)
        {
            var brandsQuery = from b in db.Brands
                              orderby b.BrandName
                              select b;
            ViewBag.BrandId = new SelectList(brandsQuery, "Id", "BrandName", selectedBrand);
        }

        private void PopulateCharacteristicsDropDownList(object selectedCharacteristic = null)
        {
            var characteristicQuery = from c in db.Characteristics
                                      orderby c.CharacName
                                      select c;
            ViewBag.CharacteristicId = new SelectList(characteristicQuery, "Id", "CharacName", selectedCharacteristic);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Add([Bind(Include = "CategoryId,BrandId,Series,Model,Price,Left,Description")]Product product)
        {
            if (product.Price == 0)
            {
                ModelState.AddModelError("", "Стоимость товара не может быть равна 0");
                PopulateBrandsDropDownList();
                PopulateCategoriesDropDownList();
                return View(product);
            }
            else
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "Product", action = "AddCharacteristics", id = product.Id });
            }
        }

        public static int AddCharProductId = 0;

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult AddCharacteristics(int id)
        {
            if (id == 0) return HttpNotFound();
            else
            {

                Product product = db.Products.Find(id);
                if (product == null) return HttpNotFound();
                else
                {
                    AddCharProductId = id;
                    ViewBag.ProductId = product.Id;
                    ViewBag.Characteristic = db.Characteristics.OrderBy(i=>i.CharacName);
                }
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult AddCharacteristics([Bind(Include = "ProductId,CharacteristicId,Value")]CharacteristicToProduct characteristicToProduct, bool exit)
        {
            if (!exit)
            {
                characteristicToProduct.ProductId = AddCharProductId;
                db.CharacteristicToProducts.Add(characteristicToProduct);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "Product", action = "SetImage", id = AddCharProductId });
            }
            else
            {
                characteristicToProduct.ProductId = AddCharProductId;
                db.CharacteristicToProducts.Add(characteristicToProduct);
                db.SaveChanges();
                return RedirectToRoute(new { controller = "Product", action = "AddCharacteristics", id = AddCharProductId });
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult SetImage(int id)
        {
            AddCharProductId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult SetImage(HttpPostedFileBase upload)
        {

            if (upload != null)
            {

                string fileNamePath = "/Content/productImages/" + AddCharProductId + upload.FileName;

                // сохранили

                upload.SaveAs(Server.MapPath(fileNamePath));

                //сохраняем в БД путь к новому файлу
                Product product = db.Products.Find(AddCharProductId);
                product.PictureURL = fileNamePath;
                db.SaveChanges();
            }

            return RedirectToRoute(new { controller = "Product", action = "Detail", id = AddCharProductId });
        }
        public static string refer = null;
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            refer = Request.UrlReferrer.ToString();
            Product product =  db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult Delete(Product product)
        {
            
            try
            {
                db.Entry(product).State = EntityState.Deleted;
                db.SaveChangesAsync();
                if (!refer.Contains("Detail"))
                {
                    return Redirect(refer);
                }
                else
                {
                    return RedirectToRoute(new { controller = "Home", action = "Index" });
                }
                
            }

            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Невозможно удалить, обратитесь к администратору системы.");
                return View(product);
            }
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult ProductList()
        {
            IEnumerable<Product> products = db.Products.OrderByDescending(i=>i.Id);
            return View(products.ToList());
        }

        [HttpGet]
        public ActionResult Detail(int? id)
        {
            if (id == null) return HttpNotFound();
            else
            {
                Product products = db.Products.Find(id);
                if (products == null) return HttpNotFound();
                else
                {
                    ViewBag.Title = "Купить " + products.Category.CategoryName + " " + products.Brand.BrandName + " " + products.Series +
                        " " + products.Model;
                    return View(products);
                }
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
            Product product = db.Products.Find(id);
            if (product != null)
            {
                ViewBag.CategoryId = new SelectList(db.Categories, "Id", "CategoryName", product.CategoryId);
                ViewBag.BrandId = new SelectList(db.Brands, "Id", "BrandName", product.BrandId);
                return View(product);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        //public ActionResult Edit(Product product)
        //{
        //    db.Entry(product).State = EntityState.Modified;
        //    db.SaveChanges();
        //    return RedirectToRoute(new { controller = "Product", action = "Detail", id = product.Id });
        //}

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteChar(int id)
        {
            CharacteristicToProduct characteristicToProduct = db.CharacteristicToProducts.Find(id);
            if (characteristicToProduct != null)
            {
                db.Entry(characteristicToProduct).State = EntityState.Deleted;
                db.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }
            return HttpNotFound();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditChar(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            CharacteristicToProduct characteristicToProduct = db.CharacteristicToProducts.Find(id);
            if (characteristicToProduct != null)
            {
                ViewBag.ProductId = new SelectList(db.Products, "Id", "Model", characteristicToProduct.ProductId);
                ViewBag.CharacteristicId = new SelectList(db.Characteristics, "Id", "CharacName", characteristicToProduct.CharacteristicId);
                return View(characteristicToProduct);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult EditChar(CharacteristicToProduct characteristicToProduct)
        {
            db.Entry(characteristicToProduct).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToRoute(new { controller = "Product", action = "Edit", id = characteristicToProduct.ProductId });
        }

    }
}