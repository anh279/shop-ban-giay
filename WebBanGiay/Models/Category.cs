using System.ComponentModel.DataAnnotations;

namespace WebBanGiay.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(200)]
        public string? CategoryName { get; set; }
        [Required]
        [StringLength(200)]
        public string? CategoryImage { get; set; }

        public int CategoryTotalProducts { get; set; }

        public int CategoryOrder { get; set; }
    }
}
