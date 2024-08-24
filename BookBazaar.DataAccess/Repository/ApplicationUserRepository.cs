using BookBazaar.DataAccess.Data;
using BookBazaar.Models;
using BookBazaar.Repository.IRepository;

namespace BookBazaar.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        } 
    }
}
