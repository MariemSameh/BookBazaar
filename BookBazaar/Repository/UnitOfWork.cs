using BookBazaar.Data;
using BookBazaar.Repository.IRepository;

namespace BookBazaar.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
        }
        public ICategoryRepository Category { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
