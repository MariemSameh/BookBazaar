using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

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
		public DateOnly? dateCreated { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
