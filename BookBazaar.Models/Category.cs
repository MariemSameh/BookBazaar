using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BookBazaar.Models
{
    public class Category
    {
        [Key]
        public int categoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
