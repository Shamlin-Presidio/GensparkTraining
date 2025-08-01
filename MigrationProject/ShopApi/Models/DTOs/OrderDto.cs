using System.ComponentModel.DataAnnotations;

namespace ShopApi.Dtos
{
    public class OrderResponseDto
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderedAt { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class OrderCreateDto
    {
        [Required]
        public string CustomerName { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
    }

    public class OrderUpdateDto
    {
        // [Required]
        // public string CustomerName { get; set; }

        // [Required]
        // public decimal TotalAmount { get; set; }
        public string? Status { get; set; }
    }
}