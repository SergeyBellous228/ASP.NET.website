namespace WebHike.Areas.Admin.Models.Product;

public class ProductDetailVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Price { get; set; } = null!;
    public string CategoryName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<string> Images { get; set; } = null!;
}
