using System.ComponentModel.DataAnnotations;

namespace ASPNET_SHAH.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; }
        [Required, MaxLength(100)]
        public string Name { get; set; }

        public List<Product> Product { get; set; } = [];
    }
}
