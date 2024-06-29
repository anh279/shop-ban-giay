using System.ComponentModel.DataAnnotations;

namespace WebBanGiay.Models
{
    public class Color
    {
        [Key]
        public int ColorId { get; set; }
        [Required]
        [StringLength(20)]
        public string? ColorName { get; set; }
    }
}
