using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebHike.Constants;
using WebHike.Data;
using WebHike.Models.Cart;

namespace WebHike.Controllers;

public class CartController(HikeDbContext hikeDbContext) : Controller
{
    public IActionResult Index()
    {
        var model = HttpContext.Session
            .GetObject<List<CartItemModel>>(Carts.CartId)
            ?? [];
        return View(model);
    }

    [HttpPost]
    public IActionResult AddToCart(int productId, int quantity = 1)
    {
        //Додамо товари в Session
        var cart = HttpContext.Session
            .GetObject<List<CartItemModel>>(Carts.CartId)
            ?? [];
        var item = cart.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            item.Quantity += quantity;//Збільшуємо кількість
        }
        else
        {
            var prod = hikeDbContext.Products
                .Include(x => x.ProductImages)
                .Include(x => x.Category)
                .SingleOrDefault(x => x.Id == productId);
            item = new CartItemModel
            {
                ProductId = productId,
                Name = prod.Name,
                CategoryName = prod.Category.Name,
                Price = prod.Price,
                Quantity = quantity,
                Image = prod.ProductImages
                    .OrderBy(x => x.Order)
                    .FirstOrDefault()?.Name ?? "default.jpg"
            };
            cart.Add(item); //додали елемент в кошик
        }

        //Оновляємо кошик із товарами
        HttpContext.Session.SetObject(Carts.CartId, cart);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult IncreaseQuantity(int productId)
    {
        var cart = HttpContext.Session
            .GetObject<List<CartItemModel>>(Carts.CartId)
            ?? [];

        var item = cart.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            item.Quantity++; //Збільшуємо кількість
        }

        HttpContext.Session.SetObject(Carts.CartId, cart);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult DecreaseQuantity(int productId)
    {
        var cart = HttpContext.Session
            .GetObject<List<CartItemModel>>(Carts.CartId)
            ?? [];

        var item = cart.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            item.Quantity--; //Зменшуємо кількість

            if (item.Quantity <= 0)
            {
                cart.Remove(item); //Видаляємо товар, якщо кількість закінчилась
            }
        }

        HttpContext.Session.SetObject(Carts.CartId, cart);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        var cart = HttpContext.Session
            .GetObject<List<CartItemModel>>(Carts.CartId)
            ?? [];

        var item = cart.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            cart.Remove(item); //Видаляємо товар з кошика
        }

        HttpContext.Session.SetObject(Carts.CartId, cart);

        return RedirectToAction(nameof(Index));
    }
}