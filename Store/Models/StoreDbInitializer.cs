using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Store.Models
{
    public class StoreDbInitializer : DropCreateDatabaseIfModelChanges<StoreContext>
    {
        protected override void Seed(StoreContext db)
        {
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });
            db.Categories.Add(new Category { CategoryName = "" });


            db.Brands.Add(new Brand { BrandName = "" });
            db.Brands.Add(new Brand { BrandName = "" });
            db.Brands.Add(new Brand { BrandName = "" });
            db.Brands.Add(new Brand { BrandName = "" });
            db.Brands.Add(new Brand { BrandName = "" });
            db.Brands.Add(new Brand { BrandName = "" });

            db.Characteristics.Add(new Characteristic { CharacName = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "", Unit = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "", Unit = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "", Unit = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "", Unit = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "", Unit = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "", Unit = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "", Unit = "" });
            db.Characteristics.Add(new Characteristic { CharacName = "", Unit = "" });

            db.Products.Add(new Product { CategoryId = 1, BrandId = 4, Price = 11290, Series = "", Model = "" });
            db.Products.Add(new Product { CategoryId = 2, BrandId = 2, Price = 4200, Model = "" });
            db.Products.Add(new Product { CategoryId = 3, BrandId = 3, Price = 7420, Series = "",
                Model = "", Description = ""});
            db.SaveChanges();

            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 1, CharacteristicId = 1, Value = ""});
            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 1, CharacteristicId = 2, Value = "" });
            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 1, CharacteristicId = 3, Value = "" });

            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 2, CharacteristicId = 1, Value = "" });
            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 2, CharacteristicId = 7, Value = "" });
            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 2, CharacteristicId = 8, Value = "" });
            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 2, CharacteristicId = 11, Value = "" });

            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 3, CharacteristicId = 14, Value = "" });
            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 3, CharacteristicId = 11, Value = "" });
            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 3, CharacteristicId = 15, Value = "" });
            db.CharacteristicToProducts.Add(new CharacteristicToProduct { ProductId = 3, CharacteristicId = 13, Value = "" });

            base.Seed(db);
        }
    }
}