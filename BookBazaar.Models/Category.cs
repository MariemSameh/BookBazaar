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
        [MinLength(2, ErrorMessage = "Name mustn't be less than 2 charecters.")]
        [MaxLength(1000, ErrorMessage = "Name mustn't exeed 1000 charecters.")]
        public string Description { get; set; }
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    }
}
