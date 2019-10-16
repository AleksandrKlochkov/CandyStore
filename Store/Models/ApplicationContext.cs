using System.Data.Entity;
using Store.Models;
using Microsoft.AspNet.Identity.EntityFramework;

public class ApplicationContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationContext() : base("StoreContext") { }

    public static ApplicationContext Create()
    {
        return new ApplicationContext();
    }

}