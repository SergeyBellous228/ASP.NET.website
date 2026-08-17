using Microsoft.AspNetCore.Mvc;
using WebHike.Areas.Admin.Models.Category;
using WebHike.Data;
using WebHike.Data.Entities;
using WebHike.Interfaces;
 

namespace WebHike.Areas.Admin.Controllers;

[Area("Admin")]

public class CategoriesController(HikeDbContext hikeDbContext, IImageService imageService, IConfiguration configuration) : Controller
{
    public IActionResult Index()
    {
        string path = configuration.GetRequiredSection("ImagesDir").Get<string>() ?? "myimages";
        var sizes = configuration.GetRequiredSection("ImageSizes").Get<List<int>>()
            ?? throw new InvalidOperationException("ImageSizes not found");
        var list = hikeDbContext.Categories
            .Select(c => new CategoryItemVM
            {
                Id = c.Id,
                Name = c.Name,
                Image = c.Image ?? "default.jpg"
            })
            .ToList();

        return View(list);
    }

    [HttpGet]

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task< IActionResult> Create(CategoryCreateVM model)
    {
        if (ModelState.IsValid)
        {
            CategoryEntity categoryEntity = new CategoryEntity();
            categoryEntity.Name = model.Name;
            categoryEntity.Slug = model.Slug;
            categoryEntity.Image = "default.jpg";

            if (model.Image != null)
            {
                //string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "myimages");

                // Фото стискається на сервері (ImageSharp): зменшується до
                // розумного розміру та перекодовується у JPEG з компресією,
                // тож 2 МБ з телефону не стають 2 МБ на сайті.
                var fileName = await imageService.SaveOptimizedImageAsync(model.Image);

                categoryEntity.Image = fileName; // В БД зберігаю назву файла

            }

            hikeDbContext.Categories.Add(categoryEntity);
            hikeDbContext.SaveChanges();

            return Redirect(nameof(Index)); // Повертаюся на список категорій
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var cat = hikeDbContext.Categories.SingleOrDefault(x => x.Id == id);
        if (cat == null)
            return NotFound();

        //string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        await imageService.RemoveImageAsync(cat.Image);

        hikeDbContext.Categories.Remove(cat);
        await hikeDbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
