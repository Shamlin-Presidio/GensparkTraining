using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{
    public class ContactUs
    {
        [Key]
        public int ContactUsId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
