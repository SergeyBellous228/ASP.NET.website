using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebHike.Data.Entities;

[Table("tblProductImages")]
public class ProductImageEntity
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Порядковий номер у списку
    /// </summary>
    public short Order { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    [ForeignKey(nameof(Product))]

    public int ProfitId { get; set; }
    public ProductEntity Product { get; set; } = null!;
}