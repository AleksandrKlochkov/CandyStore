using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Characteristic
    {
        public int Id { get; set; }
        [Required]
        public string CharacName { get; set; }
        public string Unit { get; set; }

        public ICollection<CharacteristicToProduct> CharacteristicToProducts { get; set; }
    }
}