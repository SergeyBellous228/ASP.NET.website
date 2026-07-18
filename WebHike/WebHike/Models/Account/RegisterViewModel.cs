using System.ComponentModel.DataAnnotations;

namespace WebHike.Models.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Необхідно вказати Email")]
    [EmailAddress(ErrorMessage = "Некоректний формат Email")]
    [Display(Name = "Email")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Необхідно вказати ім'я")]
    [StringLength(100, ErrorMessage = "Ім'я не може перевищувати {1} символів")]
    [Display(Name = "Ім'я")]
    public string FirstName { get; set; } = null!;
    [Required(ErrorMessage = "Необхідно вказати прізвище")]
    [StringLength(100, ErrorMessage = "Прізвище не може перевищувати {1} символів")]
    [Display(Name = "Прізвище")]
    public string LastName { get; set; } = null!;
    [Required(ErrorMessage = "Необхідно вказати пароль")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль має містити щонайменше {2} символів")]
    [Display(Name = "Пароль")]
    public string Password { get; set; } = null!;
    [Required(ErrorMessage = "Необхідно підтвердити пароль")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Паролі не співпадають")]
    [Display(Name = "Підтвердження паролю")]
    public string PasswordConfirm { get; set; } = null!;
    [Required(ErrorMessage = "Необхідно завантажити фото профілю")]
    [Display(Name = "Фото профілю")]
    [DataType(DataType.Upload)]
    public IFormFile Image { get; set; } = null!;
}