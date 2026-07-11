using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHike.Data.Entities;

[Table("tblCategories")]
public class CategoryEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string Name { get; set; } = null!;

    [StringLength(100)]
    public string? Image { get; set; } = string.Empty;

    public string Slug { get; set; } = null!;

    public ICollection<ProductEntity> Products { get; set; } = null!;
}