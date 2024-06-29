using System.ComponentModel.DataAnnotations;

namespace WebBanGiay.Models
{
    public class Type
    {
        [Key]
        public int TypeId { get; set; }
        [Required]
        [StringLength(200)]
        public string? TypeName { get; set; }
        [Required]
        [StringLength(200)]
        public string? TypeImage { get; set; }
    }
}
