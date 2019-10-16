using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class Basket
    {
        public int Id { get; set; }
        public string AccountId { get; set; }
        public int ProductId { get; set; }
        public int AmountOfProduct { get; set; }
        public DateTime DateTimeOrder { get; set; } = DateTime.Now;

        public Product Product { get; set; }
    }
}