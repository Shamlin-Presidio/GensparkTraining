using System;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{
    // public class Order
    // {
    //     [Key]
    //     public int OrderId { get; set; }

    //     [Required]
    //     public DateTime OrderDate { get; set; }

    //     public string? Status { get; set; }

    //     public int UserId { get; set; }

    //     public User User { get; set; }

    //     public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    // }

    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string? Status { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }

        public string PaymentType { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
