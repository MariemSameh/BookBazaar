using BookBazaar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookBazaar.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverTypes { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasData(
                new Company { Id = 2, Name = "Dar Afnan", StreetAddress = "New Egypt", City = "Cairo", State = "Egypt", PostalCode = "312", PhoneNumber = "111222333"},
				new Company { Id = 3, Name = "Dar Arafa", StreetAddress = "Cairo", City = "Cairo", State = "Egypt", PostalCode = "313", PhoneNumber = "111222555" },
				new Company { Id = 4, Name = "Dar nashr", StreetAddress = "New Egypt", City = "Cairo", State = "Egypt", PostalCode = "312", PhoneNumber = "225554477" }
				);

        }
    }
}
