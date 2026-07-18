using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebHike.Data;
using WebHike.Interfaces;
using WebHike.Services;
using WebHike.Data.Entities.Identity;
using WebHike.Constants;


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

// Перед запуском додам в БД ролі користувачів, якщо їх там немає
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<RoleEntity>>();
    var userManager = services.GetRequiredService<UserManager<UserEntity>>(); 

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
