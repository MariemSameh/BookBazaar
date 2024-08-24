using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookBazaar.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }
        public int count { get; set; }
        public double totalAmount { get; set; }

        [Required]
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }

        [Required]
        public int bookId { get; set; }
        [ForeignKey("bookId")]
        [ValidateNever]
        public Book book { get; set; }
    }
}
