using Microsoft.EntityFrameworkCore;
using WebHike.Data;
using WebHike.Interfaces;
using WebHike.Services;

namespace WebHike
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            string strConnection = builder.Configuration.GetConnectionString("MyWebHikeConnection") ?? "";

            builder.Services.AddDbContext<HikeDbContext>(opt =>
                opt.UseNpgsql(strConnection));

            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IImageService, ImageOptimizationService>();

            var app = builder.Build();

            var dirName = "images";
            var dirCurrent = Directory.GetCurrentDirectory();
            var path = Path.Combine(dirCurrent, "wwwroot", dirName);
            Directory.CreateDirectory(path);

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Main}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
