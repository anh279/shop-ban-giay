using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanGiay.Models
{

    public class Invoke
    {
        [Key]
        public int InvokeId { get; set; }
        [Required]
        [StringLength(50)]
        public string? CustomerName { get; set; } 
        [Required]
        [StringLength(200)]
        public string? Address { get; set; } 
        [Required]
        [StringLength(15)]
        public string? PhoneNumber { get; set; } 
        public DateTime OrderDate { get; set; } // Ngày đặt hàng
        public DateTime DeliveryDate { get; set; } // Ngày giao hàng
        public int DaysToDeliver { get; set; } // Số ngày giao hàng 
        public decimal InvokeTotalAmount { get; set; }
        [Required]
        [StringLength(50)]
        public string? ShippingMethod { get; set; } // Cách vận chuyển
        [Required]
        [StringLength(50)]
        public string? PaymentMethod { get; set; } // Cách thanh toán
        public decimal ShippingFee { get; set; } // Phí vận chuyển

        [ForeignKey("Status")]
        public int StatusId { get; set; }

        public Status? Status { get; set; }

        [ForeignKey("User")]
        public string? Username { get; set; }
        public User? User { get; set; }

    }
}
