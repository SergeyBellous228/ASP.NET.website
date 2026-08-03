using Microsoft.AspNetCore.Mvc;
using WebHike.Constants;
using WebHike.Models.Cart;

namespace WebHike.ViewComponents;

public class CartBadgeViewComponent : ViewComponent
{
    public IViewComponentResult Invoke()
    {
        var cart = HttpContext.Session
            .GetObject<List<CartItemModel>>(Carts.CartId)
            ?? [];

        var count = cart.Sum(x => x.Quantity);

        return View(count);
    }
}
