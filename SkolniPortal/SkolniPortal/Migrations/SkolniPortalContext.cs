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
        public DbSet<Zasedak> Zasedaky { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfiguraci modelů můžete přidat zde
            // Ensure user name is unique at the database level to prevent duplicates
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
