using BookBazaar.DataAccess.Data;
using BookBazaar.Models;
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
            Book = new BookReposatory(_context);
            Company = new CompanyReposatory(_context);
        }
        public ICategoryRepository Category { get; private set; }
        public IBookReposatory Book { get; private set; }
        public ICompanyReposatory Company { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
