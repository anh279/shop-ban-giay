using MessagePack;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanGiay.Models
{
    public class InvokeDetail
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int InvokeDetailId { get; set; }
        [ForeignKey("Invoke")]
        public int? InvokeId { get; set; }
        public Invoke? Invoke { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

    }
}
