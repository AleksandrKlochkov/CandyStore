using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Store.Models;

namespace Store.Controllers
{
    public class BasketController : Controller
    {
        StoreContext db = new StoreContext();

        [HttpGet]
        [Authorize]
        public ActionResult Basket(string userid)
        {
            if (String.IsNullOrEmpty(userid)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var basket = db.Baskets
                 .Include(p => p.Product)
                 .OrderByDescending(d => d.DateTimeOrder)
                 .Where(i => i.AccountId.Equals(userid));

            return View(basket.ToList());
        }

        [HttpGet]
        [Authorize]
        public ActionResult Add(int id, string userid)
        {
            Product product = db.Products.Find(id);
            if (product == null) return HttpNotFound();
            Basket basket = new Basket { AccountId = userid, ProductId = id, AmountOfProduct = 1, Product = product };

            var findDublicate = db.Baskets.Where(i => i.AccountId.Equals(basket.AccountId) && i.ProductId == basket.ProductId);

            if (findDublicate.Any())
            {
                return RedirectToRoute(new { controller = "Basket", action = "Basket", userid = userid });
            }
            else
            {
                db.Baskets.Add(basket);
                db.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Basket", action = "Basket", userid = userid });
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Delete(int id, string userid)
        {
            if (id == 0 || String.IsNullOrEmpty(userid)) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            Basket basket = await db.Baskets.FindAsync(id);
            if (basket == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                db.Entry(basket).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToRoute(new { controller = "Basket", action = "Basket", userid = userid });
            }

        }

    }
}