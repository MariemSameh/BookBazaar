using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookBazaar.Models
{
    public class ApplicationUser: IdentityUser
    {
        
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
		public string? StreetAddress { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
        public string? PostalCode { get; set; }
        public DateOnly? dateCreated { get; set; }
        public DateTime? LastLogin { get; set; }

        public int? companyId { get; set; }
        [ForeignKey("companyId")]
        [ValidateNever]
        public Company Company { get; set; }
    }
}
