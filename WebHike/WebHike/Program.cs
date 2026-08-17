using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebHike.Constants;
using WebHike.Data;
using WebHike.Data.Entities.Identity;
using WebHike.Interfaces;
using WebHike.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string strConnection = builder.Configuration.GetConnectionString("MyWebHikeConnection") ?? "";

builder.Services.AddDbContext<HikeDbContext>(opt => opt.UseNpgsql(strConnection));

builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
})
.AddEntityFrameworkStores<HikeDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IImageService, ImageOptimizationService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

//var dirName = "images";
//var dirCurrent = Directory.GetCurrentDirectory();
//var path = Path.Combine(dirCurrent, "wwwroot", dirName);
//Directory.CreateDirectory(path); //автоматично стоврить images

try
{
    var myImage = builder.Configuration.GetRequiredSection("ImagesDir").Get<string>() ?? "myimages";
    string path = Path.Combine(Directory.GetCurrentDirectory(), myImage);
    Directory.CreateDirectory(path); //автоматично стоврить images

    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(path),
        RequestPath = $"/{myImage}"
    });
}
catch (Exception ex)
{
    Console.WriteLine("Помилка запуску" + ex.Message);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Main}/{action=Index}/{id?}")
//    .WithStaticAssets();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
      name: "admin_area",
      areaName: "Admin",
      pattern: "admin/{controller=Dashboards}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Main}/{action=Index}/{id?}"
    );
});

// Перед запуском додам в БД ролі користувачів, якщо їх там немає
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<RoleEntity>>();
    var userManager = services.GetRequiredService<UserManager<UserEntity>>();
    var dbContext = services.GetRequiredService<HikeDbContext>();

    //Коли запускається проект він перевіряє БД, якщо БД пуста то він автоматично робить БД
    //на основі міграцій, які є у проекті
    await dbContext.Database.MigrateAsync();

    if (!roleManager.Roles.Any())
    {
        foreach (var roleName in Roles.ListRoles())
        {
            await roleManager.CreateAsync(new RoleEntity { Name = roleName });
        }
    }

    if (!userManager.Users.Any())
    {
        var user = new UserEntity
        {

            Email = "admin@gmail.com",
            UserName = "admin@gmail.com",
            FirstName = "Павло",
            LastName = "Зібров",
            Image = "default.jpg"
        };

        await userManager.CreateAsync(user, "Qwerty1-");
        await userManager.AddToRoleAsync(user, Roles.Admin);
    }
}

app.Run();

