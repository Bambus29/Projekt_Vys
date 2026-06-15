using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SkolniPortal.Models
{
    public class ApplicationDbContext : SkolniPortalContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Zasedak> Zasedaky { get; set; }



    }
}
