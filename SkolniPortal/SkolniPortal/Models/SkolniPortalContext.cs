using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace SkolniPortal.Models
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
            base.OnModelCreating(modelBuilder);

            // Ensure user name is unique at the database level
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            // ValueConverter pro List<string> -> JSON string
            var listToJsonConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>());

            modelBuilder.Entity<Zasedak>()
                .Property(z => z.Mista)
                .HasConversion(listToJsonConverter)
                .HasColumnType("nvarchar(max)");
        }
    }
}