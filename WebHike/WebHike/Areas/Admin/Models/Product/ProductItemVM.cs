namespace WebHike.Areas.Admin.Models.Product;

//Для Адміна моделі закінчуються на VM
public class ProductItemVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Price { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public List<string> Images { get; set; } = null!;
}
