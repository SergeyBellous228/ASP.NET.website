using System.ComponentModel.DataAnnotations;

namespace WebHike.Models.Product;

public class CreateProductViewModel
{
    [Required(ErrorMessage = "Вкажіть назву")]
    [Display(Name = "Назва продукта")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Вкажіть опис")]
    [Display(Name = "Опис продукта")]
    public string? Description { get; set; } = String.Empty;

    [Required(ErrorMessage = "Вкажіть ціну")]
    [Display(Name = "Ціна продукта")]
    public string Price { get; set; } = null!;

    [Required(ErrorMessage = "Вкажіть Slug")]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = null!;

    [Required(ErrorMessage = "Вкажіть  Category")]
    [Display(Name = "Category")]
    public string CategoryName { get; set; } = null!;

    public List<CreateProductImageViewModel> Images { get; set; } = null!;
}