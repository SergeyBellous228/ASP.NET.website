using Microsoft.AspNetCore.Mvc;
using WebHike.Data;
using WebHike.Data.Entities;
using WebHike.Interfaces;
using WebHike.Models.Product;

namespace WebHike.Controllers;

public class ProductsController(HikeDbContext hikeDbContext,
    IImageService imageService) : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult ProdCreate()
    {
        //отримую із БД назва категорій
        //Кладу у спеціальну динамічну колекцію, яка буде доступна на View
        ViewBag.Categories = hikeDbContext
            .Categories.Select(x => x.Name).ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ProdCreate(CreateProductViewModel model)
    {
        //отримую із БД назва категорій
        //Кладу у спеціальну динамічну колекцію, яка буде доступна на View
        if (ModelState.IsValid)
        {
            var cat = hikeDbContext.Categories.SingleOrDefault(x => x.Name == model.CategoryName);
            var entity = new ProductEntity
            {
                Name = model.Name,
                CategoryId = cat.Id,
                Description = model.Description,
                Price = 0.0M,
                Slug = model.Slug
            };
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

            var savedImages = await Task.WhenAll(
                model.Images.Select(async image => new ProductImageEntity
                {
                    Name = await imageService.SaveOptimizedImageAsync(image.Base64Image, folderPath),
                    Order = image.Order
                })
            );
            entity.ProductImages = savedImages.ToList();

            hikeDbContext.Products.Add(entity);
            await hikeDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        ViewBag.Categories = hikeDbContext
            .Categories.Select(x => x.Name).ToList();
        return View(model);
    }
}