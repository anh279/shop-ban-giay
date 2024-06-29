using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBanGiay.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [StringLength(200)]
        public string? ProductName { get; set; }
        [Required]
        [StringLength(4000)]
        public string? ProductDescription { get; set; }
        [ForeignKey("Category")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
        [ForeignKey("Type")]
        public int? TypeId { get; set; }
        public Type? Type { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal ProductPrice { get; set; }
        [Required]
        [Column(TypeName = "decimal(2,2)")]
        public decimal ProductDiscount { get; set; }
        [StringLength(200)]
        public string? ProductImage { get; set; }
        [ForeignKey("Size")]
        public int SizeId { get; set; }
        public Size? Size { get; set; }
        [ForeignKey("Color")]
        public int ColorId { get; set; }
        public Color? Color { get; set; }
        public bool IsHot { get; set; }
        public bool IsNew { get; set; }
    }
}
