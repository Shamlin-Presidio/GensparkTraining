using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApi.Models
{
    public class Model
    {
        public Model()
        {
            Products = new HashSet<Product>();
        }

        [Key]
        public int ModelId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model1 { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
