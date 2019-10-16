using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Required]
        public string BrandName { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}