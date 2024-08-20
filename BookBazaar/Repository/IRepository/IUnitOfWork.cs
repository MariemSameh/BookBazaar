namespace BookBazaar.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IBookReposatory Book {  get; }
        ICompanyReposatory Company { get; }

        void Save();
    }
}
