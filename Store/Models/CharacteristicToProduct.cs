using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class CharacteristicToProduct
    {
        public int Id { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int CharacteristicId { get; set; }
        [Required]
        public string Value { get; set; }

        public virtual Product Products { get; set; } 
        public virtual Characteristic Characteristics { get; set; }

    }
}