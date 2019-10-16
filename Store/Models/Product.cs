using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int BrandId { get; set; }
        public string Series { get; set; }
        public string Model { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public DateTime DateAdded { get; set; } = DateTime.Now;
        [Required]
        public int Left { get; set; } = 1;
        public string Description { get; set; }
        public string PictureURL { get; set; } = "/Content/productImages/01.jpg";

        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual ICollection<CharacteristicToProduct> CharacteristicToProducts { get; set; }

    }
}