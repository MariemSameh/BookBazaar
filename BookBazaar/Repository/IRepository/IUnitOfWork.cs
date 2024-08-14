namespace BookBazaar.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IBookReposatory Book {  get; }

        void Save();
    }
}
