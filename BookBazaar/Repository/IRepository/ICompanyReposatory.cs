using BookBazaar.Models;

namespace BookBazaar.Repository.IRepository
{
    public interface ICompanyReposatory : IRepository<Company>
    {
        void Update(Company company);
    }
}
