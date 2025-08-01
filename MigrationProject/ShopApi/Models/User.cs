using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }

        public string? Password { get; set; }

        public string? FullName { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
