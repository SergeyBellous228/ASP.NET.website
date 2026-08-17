namespace WebHike.Areas.Admin.Models.Category;

//Для відображення категорій на сайті
public class CategoryItemVM
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
}