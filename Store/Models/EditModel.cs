using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store.Models
{
    public class EditModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int PostalCode { get; set; }
        public string Address { get; set; }
    }
}