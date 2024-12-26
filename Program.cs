using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Olimpiada.Models.Olimpiada;

namespace Olimpiada
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<OlympicsContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("OlympicsDatabase")));


            builder.Services.AddControllersWithViews();
            
            using (var context = new OlympicsContext(builder.Services.BuildServiceProvider().GetService<DbContextOptions<OlympicsContext>>()))
            {
                var people = context.People.ToList();
                Console.WriteLine($"Found {people.Count} people in the database.");
            }


            var app = builder.Build();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }

}