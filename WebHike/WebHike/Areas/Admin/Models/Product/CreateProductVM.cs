using System.ComponentModel.DataAnnotations;

namespace WebHike.Areas.Admin.Models.Product;

public class CreateProductVM
{
    [Required(ErrorMessage = "Вкажіть назву")]
    [Display(Name = "Назва продукта")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Вкажіть опис")]
    [Display(Name = "Опис продукта")]
    public string? Description { get; set; } = String.Empty;
    [Required(ErrorMessage = "Вкажіть ціну")]
    [Display(Name = "Ціна продукта")]
    [DataType(DataType.Currency)]
    public string Price { get; set; } = null!;
    [Required(ErrorMessage = "Вкажіть Slug")]
    [Display(Name = "Slug")]
    public string Slug { get; set; } = null!;
    [Required(ErrorMessage = "Вкажіть Категорію")]
    [Display(Name = "Категорія")]
    public string CategoryName { get; set; } = null!;
    public List<CreateProductImageVM> Images { get; set; } = null!;
}
