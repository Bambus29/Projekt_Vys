using Microsoft.EntityFrameworkCore;
using SkolniPortal.Models;

namespace SkolniPortal.Migrations
{
    public class SkolniPortalContext : DbContext
    {
        public SkolniPortalContext(DbContextOptions<SkolniPortalContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Konfiguraci modelů můžete přidat zde
        }
    }
}
