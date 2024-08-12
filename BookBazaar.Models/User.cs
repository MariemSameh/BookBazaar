using System.ComponentModel.DataAnnotations;

namespace BookBazaar.Models
{
    public class User
    {
        [Key]
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public int phoneNumber { get; set; }
        public DateOnly dateCreated { get; set; }
        public DateTime LastLogin { get; set; }
    }
}
