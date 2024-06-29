using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebBanGiay.Models;

namespace WebBanGiay.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Invoke> Invoke { get; set; }
        public DbSet<InvokeDetail> InvokeDetail { get; set; }
        public DbSet<WebBanGiay.Models.User>? User { get; set; }
        public DbSet<WebBanGiay.Models.Type>? Type { get; set; }

        public DbSet<WebBanGiay.Models.Status>? Status { get; set; }

    }
}