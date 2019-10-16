using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Store.Models
{
    public class StoreContext: DbContext
    {
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<CharacteristicToProduct> CharacteristicToProducts { get; set; }
    }
}