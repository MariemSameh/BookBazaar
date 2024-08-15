using BookBazaar.Models;
using BookBazaar.Models.ViewModels;
using BookBazaar.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace BookBazaar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BookController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Book> books = _unitOfWork.Book.GetAll(includeProperties:"Category").ToList();
            return View(books);
        }

        public IActionResult Upsert(int? id)
        {
            
            BookVM bookVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.categoryId.ToString()
				}),
                book = new Book()
			};
            if(id == null || id == 0)
            {
                return View(bookVM);
            }
            else
            {
                bookVM.book = _unitOfWork.Book.GetFirstOrDefault(u => u.bookId == id);
				return View(bookVM);
			}
            
        }

        [HttpPost]
        public IActionResult Upsert(BookVM bookVM, IFormFile? file)
        {
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string bookPath = Path.Combine(wwwRootPath, @"images\Book");
                    if (bookVM.book.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, bookVM.book.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(bookPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        fileStream.Dispose();

                    }
                    bookVM.book.ImageUrl = @"\images\Book\" + fileName;
                }
                else
                {
                    bookVM.book.ImageUrl = @"\images\NoImage.png";
				}
                

                if(bookVM.book.bookId == 0)
                {
                    _unitOfWork.Book.Add(bookVM.book);
                }
                else
                {
					_unitOfWork.Book.Update(bookVM.book);
				}
                _unitOfWork.Save();
                TempData["success"] = "Book added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                bookVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.categoryId.ToString()
                });
                return View(bookVM);
            }
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Book> books = _unitOfWork.Book.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = books});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var bookToBeDeleted = _unitOfWork.Book.GetFirstOrDefault(u => u.bookId == id);
            if (bookToBeDeleted == null)
            {
                return Json(new { Success = false, message = "Error While deleting" });
            }
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, bookToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Book.Remove(bookToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Book Deleted Successfully." });
        }
        #endregion
    }
}
