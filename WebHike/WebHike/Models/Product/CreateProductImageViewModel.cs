namespace WebHike.Models.Product;

public class CreateProductImageViewModel
{
    public string Base64Image { get; set; } = string.Empty;

    public short Order { get; set; }
}