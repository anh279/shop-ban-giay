using System.ComponentModel.DataAnnotations;

namespace WebBanGiay.Models
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        [Required]
        [StringLength(200)]
        public string? StatusName { get; set; }
        
    }
}
