using BookBazaar.DataAccess.Data;
using BookBazaar.Models;
using BookBazaar.Repository.IRepository;

namespace BookBazaar.Repository
{
    public class CompanyReposatory : Repository<Company>, ICompanyReposatory
    {
        private ApplicationDbContext _db;

        public CompanyReposatory(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Company company)
        {
            _db.Update(company);
        }
    }
}
