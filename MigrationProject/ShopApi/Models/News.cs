using System;
using System.ComponentModel.DataAnnotations;

namespace ShopApi.Models
{
    public class News
    {
        [Key]
        public int NewsId { get; set; }

        [Required]
        public string Title { get; set; }

        public string? ShortDescription { get; set; } 
        public string? Content { get; set; }

        public string? Image { get; set; }             

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;  

        public string? Status { get; set; }            

        
        public int? UserId { get; set; }               
        public User? User { get; set; }                
    }
}
