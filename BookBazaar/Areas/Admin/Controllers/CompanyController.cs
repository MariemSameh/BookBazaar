using BookBazaar.Models;
using BookBazaar.Repository.IRepository;
using BookBazaar.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookBazaar.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }

        public IActionResult Upsert(int? id)
        {

            Company company = new();
            if(id == null || id == 0)
            {
                return View(company);
            }
            else
            {
				company = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
				return View(company);
			}
            
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            
            if (ModelState.IsValid)
            {
                
                if(company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
					TempData["success"] = "Company added successfully";
				}
				else
                {
					_unitOfWork.Company.Update(company);
					TempData["success"] = "Company Updated successfully";
				}
				_unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(company);
            }
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { Success = false, message = "Error While deleting" });
            }
            
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Company Deleted Successfully." });
        }
        #endregion
    }
}
