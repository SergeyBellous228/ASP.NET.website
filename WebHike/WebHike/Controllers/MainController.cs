using Microsoft.AspNetCore.Mvc;
using WebHike.Data;
using WebHike.Data.Entities;
using WebHike.Interfaces;
using WebHike.Models.Category;
 

namespace WebHike.Controllers;

public class MainController(HikeDbContext hikeDbContext, IImageService imageService) : Controller
{
    public IActionResult Index()
    {
        var list = hikeDbContext.Categories.ToList();
        return View(list);
    }

    [HttpGet]

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task< IActionResult> Create(CategoryCreateViewModel model)
    {
        if (ModelState.IsValid)
        {
            CategoryEntity categoryEntity = new CategoryEntity();
            categoryEntity.Name = model.Name;
            categoryEntity.Slug = model.Slug;
            categoryEntity.Image = "default.jpg";

            if (model.Image != null)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                // Фото стискається на сервері (ImageSharp): зменшується до
                // розумного розміру та перекодовується у JPEG з компресією,
                // тож 2 МБ з телефону не стають 2 МБ на сайті.
                var fileName = await imageService.SaveOptimizedImageAsync(model.Image, folderPath);

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

        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        await imageService.RemoveImageAsync(cat.Image, folderPath);

        hikeDbContext.Categories.Remove(cat);
        await hikeDbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
