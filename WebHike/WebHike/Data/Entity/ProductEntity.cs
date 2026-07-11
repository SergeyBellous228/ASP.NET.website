using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHike.Data.Entities;

[Table("tblProducts")]
public class ProductEntity
{
    [Key]
    public int Id { get; set; }
    [StringLength(500)]
    public string Name { get; set; } = null!;
    [StringLength(10000)]
    public string? Description { get; set; } = String.Empty;
    public decimal Price { get; set; }
    [StringLength(500)]
    public string Slug { get; set; } = null!;
    [ForeignKey(nameof(Category))]
    public int CategoryId { get; set; }
    public CategoryEntity Category { get; set; } = null!;
    public bool IsDeleted { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public ICollection<ProductImageEntity> ProductImages { get; set; } = null!;
}
