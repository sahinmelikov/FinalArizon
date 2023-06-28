using FinalArizon.Models;
using FinalArizon.Models.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalArizon.DAL
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions <AppDbContext> options) : base(options)
        {
        }
       
        public DbSet<Product>? Products { get; set; }
      
        public DbSet<ParentsCategory>?ParentsCategories  { get; set; }
        public DbSet<Slider>Sliders { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Model> Models { get; set; }

    }
}
