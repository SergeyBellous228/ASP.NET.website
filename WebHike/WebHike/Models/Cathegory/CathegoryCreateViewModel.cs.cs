using System.ComponentModel.DataAnnotations;

namespace WebHike.Models.Category;

/// <summary>
/// Модель для передачі даних на сервер із View
/// Створення категорії
/// </summary>
public class CategoryCreateViewModel
{
    [Display(Name = "Назва категорії")]
    [Required(ErrorMessage = "Введіть назву категорії")]
    public string Name { get; set; } = null!;

    [Display(Name = "Slug категорії")]
    [Required(ErrorMessage = "Введіть slug категорії")]
    public string Slug { get; set; } = null!;

    [Display(Name = "Photo категорії")]
    [DataType(DataType.Upload)]
    public IFormFile? Image { get; set; } = null!;
}