using Microsoft.EntityFrameworkCore;
using SkolniPortal.Data;
using SkolniPortal.Models;

namespace SkolniPortal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add DbContext (use ApplicationDbContext as implementation)
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Add session services
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.IdleTimeout = TimeSpan.FromHours(8);
            });

            var app = builder.Build();
            //migrace databaze pri startu aplikace
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                // Remove duplicate users (keep the one with smallest Id) before applying migrations
                // This avoids CREATE UNIQUE INDEX failing when existing data contains duplicate Name values.
                try
                {
                    var dedupeSql = @"WITH cte AS (
  SELECT Id, ROW_NUMBER() OVER (PARTITION BY Name ORDER BY Id) AS rn
  FROM Users
)
DELETE FROM Users WHERE Id IN (SELECT Id FROM cte WHERE rn > 1);";
                    db.Database.ExecuteSqlRaw(dedupeSql);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<Program>>();
                    logger.LogError(ex, "Failed to deduplicate Users table before migrations.");
                    throw;
                }

                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
