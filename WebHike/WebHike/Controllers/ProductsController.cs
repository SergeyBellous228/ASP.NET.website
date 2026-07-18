using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
        var model = hikeDbContext.Products.
            Select(x => new ProductItemViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price.ToString("C", new CultureInfo("uk-UA")),
                CategoryName = x.Category.Name,
                Images = x.ProductImages.OrderBy(x => x.Order).Select(x => x.Name).Take(2).ToList()
            }).ToList();
        //var products = hikeDbContext.Products
        //    .Include(x => x.Category)
        //    .Include(x => x.ProductImages)
        //    .ToList();
        return View(model);
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
            string priceStr = model.Price.ToString().Trim().Replace(".", ",");
            decimal price = Decimal.Parse(priceStr, new CultureInfo("uk-UA"));
            var entity = new ProductEntity
            {
                Name = model.Name,
                CategoryId = cat.Id,
                Description = model.Description,
                Price = price,
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

    public IActionResult Details(int id)
    {
        ProductDetailViewModel? model = hikeDbContext.Products
            .Select(x => new ProductDetailViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price.ToString("C", new CultureInfo("uk-UA")),
                CategoryName = x.Category.Name,
                Description = x.Description ?? String.Empty,
                Images = x.ProductImages
                    .OrderBy(x => x.Order)
                    .Select(x => x.Name)
                    .ToList()
            })
            .SingleOrDefault(x => x.Id == id);

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var product = hikeDbContext.Products.Include(x => x.ProductImages).SingleOrDefault(x => x.Id == id);
        if (product == null)
            return NotFound();

        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        foreach(var image in product.ProductImages)
        {
            await imageService.RemoveImageAsync(image.Name, folderPath);
        }

        hikeDbContext.Products.Remove(product);
        await hikeDbContext.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}