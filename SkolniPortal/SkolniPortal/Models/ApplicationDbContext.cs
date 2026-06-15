using Microsoft.EntityFrameworkCore;

namespace SkolniPortal.Models
{
    public class ApplicationDbContext : SkolniPortalContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}
