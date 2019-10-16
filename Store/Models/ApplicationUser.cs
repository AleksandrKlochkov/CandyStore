using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Store.Models
{
        public class ApplicationUser : IdentityUser
        {
      
            public string LastName { get; set; }
      
            public string FirstName { get; set; }
        
            public int PostalCode { get; set; }
        
            public string Address { get; set; }


            public ApplicationUser()
            {
            }
        }
}