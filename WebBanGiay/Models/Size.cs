using System.ComponentModel.DataAnnotations;

namespace WebBanGiay.Models
{
    public class Size
    {
        [Key]
        public int SizeId { get; set; }
        [Required]
        [StringLength(20)]
        public string? SizeName { get; set; }
    }
}
