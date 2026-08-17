namespace WebHike.Areas.Admin.Models.Product;

public class CreateProductImageVM
{
    public string Base64Image { get; set; } = string.Empty;
    public short Order { get; set; }
}
