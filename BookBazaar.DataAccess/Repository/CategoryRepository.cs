using BookBazaar.DataAccess.Data;
using BookBazaar.Models;
using BookBazaar.Repository.IRepository;

namespace BookBazaar.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
