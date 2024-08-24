using BookBazaar.Models;

namespace BookBazaar.Repository.IRepository
{
    public interface IBookRepository : IRepository<Book>
    {
        void Update(Book obj);
    }
}
