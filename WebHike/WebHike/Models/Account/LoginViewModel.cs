using System.ComponentModel.DataAnnotations;

namespace WebHike.Models.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Необхідно вказати Email")]
    [EmailAddress(ErrorMessage = "Некоректний формат Email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Необхідно вказати пароль")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має містити щонайменше {2} символів")]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;
    [Display(Name = "Запам'ятати мене")]
    public bool RememberMe { get; set; }
    public string? ReturnUrl { get; set; }
}