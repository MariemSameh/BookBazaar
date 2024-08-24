using BookBazaar.DataAccess.Data;
using BookBazaar.Models;
using BookBazaar.Repository.IRepository;

namespace BookBazaar.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        ApplicationDbContext _context;
        
        public ICategoryRepository Category { get; private set; }
        public IBookRepository Book { get; private set; }
        public ICompanyRepository Company { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Book = new BookRepository(_context);
            Company = new CompanyRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);
            OrderDetail = new OrderDetailRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
