using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantApp.Areas.Admin.Models;

namespace RestaurantApp.Data
{
    public class AppUser : IdentityUser<int>
    {
        public int? Age { get; set; } = 0;
    }
    public class AppRole:IdentityRole<int> 
    { 

    }
    public class RestaurantDbContext : IdentityDbContext<AppUser,AppRole,int>
    {

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options)
            : base(options)
        {
        }
    }
}
