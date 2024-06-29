using System.ComponentModel.DataAnnotations;

namespace WebBanGiay.Models
{
    public class User
    {
        [Key]
        [StringLength(50)]
        public string? UserName { get; set; }
        
        [Required]
        [StringLength(50)]
        public string? UserPassword { get; set; }
        [Required]
        [StringLength(50)]
        public string? UserFullName { get; set; }
        [Required]
        [StringLength(15)]
        public string? UserPhoneNumber { get; set; }
        [Required]
        [StringLength(200)]
        public string? UserAddress { get; set; }

        [Required]
        [StringLength(20)]
        public string? UserRole { get; set; }
    }
}
