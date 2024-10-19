using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HrMangementSystem.Models;

namespace HrMangementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure the database context with a connection string
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Identity with the specified user and role types
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                // Configure password requirements here (optional)
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddControllersWithViews();
            builder.Services.AddSession(); // Add session support

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Ensure authentication middleware is used
            app.UseAuthorization(); // Ensure authorization middleware is used
            app.UseSession(); // Ensure session middleware is used

            // Configure the default route
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Staff}/{action=Login}/{id?}"); // Change this if your login controller is different

            app.Run();
        }
    }
}
