using BookBazaar.Models;

namespace BookBazaar.Repository.IRepository
{
    public interface IBookReposatory : IRepository<Book>
    {
        void Update(Book obj);
    }
}
