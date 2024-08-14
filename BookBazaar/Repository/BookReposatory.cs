using BookBazaar.DataAccess.Data;
using BookBazaar.Models;
using BookBazaar.Repository.IRepository;

namespace BookBazaar.Repository
{
    public class BookReposatory : Repository<Book>, IBookReposatory
    {
        private ApplicationDbContext _db;
        public BookReposatory(ApplicationDbContext db) :  base(db) 
        {
            _db = db;
        }
        public void Update(Book obj)
        {
			var objFromDb = _db.Books.FirstOrDefault(u => u.bookId == obj.bookId);
			if (objFromDb != null)
			{
				objFromDb.title = obj.title;
				objFromDb.ISBN = obj.ISBN;
				objFromDb.Price = obj.Price;
				objFromDb.Price50 = obj.Price50;
				objFromDb.ListPrice = obj.ListPrice;
				objFromDb.Price100 = obj.Price100;
				objFromDb.Description = obj.Description;
				objFromDb.CategoryId = obj.CategoryId;
				objFromDb.Author = obj.Author;
				if (obj.ImageUrl != null)
				{
					objFromDb.ImageUrl = obj.ImageUrl;
				}
			}
			//_db.Books.Update(obj);
		}
    }
}
