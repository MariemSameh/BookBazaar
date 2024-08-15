using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookBazaar.Models
{
    public class ShoppingCart
    {
        [Key]
        public int shoppingCartId { get; set; }

        public int bookId { get; set; }
        [ForeignKey("bookId")]
        [ValidateNever]
        public Book book { get; set; }
        [Range(1, 1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Count { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [NotMapped]
        public double Price { get; set; }
    }
}
