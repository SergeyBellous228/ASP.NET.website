using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebHike.Data;
using WebHike.Data.Entities;
using WebHike.Interfaces;
using WebHike.Models.Category;

namespace WebHike.Controllers;

public class MainController(HikeDbContext hikeDbContext,
    IConfiguration configuration,
    IImageService imageService)
    : Controller
{
    //private readonly HikeDbContext _hikeDbContext;
    //public MainController(HikeDbContext hikeDbContext)
    //{
    //    _hikeDbContext = hikeDbContext;
    //}
    //Методи у ASP.NET - звуться Action - дія
    public IActionResult Index()
    {
        ///Контролер дані передає на певну View
        ///View - це звичайна html сторінка із
        ///кодом C# - Razor View
        ///return "Привіт козаки :)";
        ///так краще не робити :(

        string path = configuration.GetRequiredSection("ImagesDir").Get<string>() ?? "myimages";
        var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>() ??
            throw new InvalidOperationException("ImageSizes not found");
        var list = hikeDbContext.Categories
            .Select(x => new CategoryItemVM
            {
                Id = x.Id,
                Name = x.Name,
                Image = $"/{path}/{x.Image}_{sizes[1]}.webp"
            })

            .ToList();
        return View(list); //Передаю дані на View - список категорій
    }
    //Метод для створення категорії нової
    [HttpGet] //Для відображення фоми
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost] //Цей метод спрацьовує коли кидає Post Request
    public async Task<IActionResult> Create(CategoryCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            CategoryEntity categoryEntity = new CategoryEntity();
            categoryEntity.Name = model.Name;
            categoryEntity.Slug = model.Slug;
            categoryEntity.Image = "default.jpg";
            try
            {
                if (model.Image != null)
                {
                    var fileName = await imageService.SaveOptimizedImageAsync(model.Image);
                    categoryEntity.Image = fileName; //в БД зберігаю назву файла
                }

                hikeDbContext.Categories.Add(categoryEntity);
                hikeDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(String.Empty, "Сталася халупа " + ex.Message);
                return View(model); // Що прийшло те іде назад
            }

            return Redirect(nameof(Index)); //Повертаюся на список категорій
        }

        return View(model); // Що прийшло те іде назад
    }

    //[HttpPost]
    //public async Task<IActionResult> Delete(int id) 
    //{
    //    var cat = hikeDbContext.Categories.SingleOrDefault(x => x.Id == id);
    //    if(cat == null)
    //        return NotFound();

    //    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
    //    await imageService.RemoveImageAsync(cat.Image, folderPath);

    //    hikeDbContext.Categories.Remove(cat);
    //    await hikeDbContext.SaveChangesAsync();

    //    return RedirectToAction(nameof(Index));
    //}
}
