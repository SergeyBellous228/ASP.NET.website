using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebHike.Constants;
using WebHike.Data.Entities.Identity;
using WebHike.Interfaces;
using WebHike.Models.Account;

namespace WebHike.Controllers;

public class AccountController(UserManager<UserEntity> userManager, IImageService imageService,
    SignInManager<UserEntity> signInManager) : Controller
{
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        var model = new LoginViewModel { ReturnUrl = returnUrl };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var result = await userManager.CheckPasswordAsync(user, model.Password);
            if (result)
            {
                await signInManager.SignInAsync(user, false);
                return Redirect(model.ReturnUrl ?? "/");
            }
        }
        ModelState.AddModelError(String.Empty, "Дані вказано не вірно!");
        return View(model);
    }
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            ModelState.AddModelError("Email", "Дана пошта уже зареєстрована");
            return View(model);
        }
        user = new UserEntity()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            UserName = model.Email,
            Image = "default.jpg"
        };

        if (model.Image != null)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var fileName = await imageService.SaveOptimizedImageAsync(model.Image, folderPath);
            user.Image = fileName;
        }

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, Roles.User);
            await signInManager.SignInAsync(user, false);
            return Redirect("/"); //Після успішної реєстарції переходимо на головну
        }
        else
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Redirect("/");
    }
}


