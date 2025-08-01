using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public string? Description { get; set; }

        // Foreign keys
        public int CategoryId { get; set; }
        public int ColorId { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public Color Color { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
