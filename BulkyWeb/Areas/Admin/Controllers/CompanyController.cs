using System.Collections.Generic;
using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;


        public CompanyController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }

        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitofwork.Company.Getall().ToList();
            
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == 0 || id == null)
            {
                //create company
                return View(new Company());
            }
            else
            {
                //udpate company
                Company companyobj = _unitofwork.Company.Get(u => u.Id == id);
                return View(companyobj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company companyobj)
        {
            
            if (ModelState.IsValid)
            {
                if (companyobj.Id == 0)
                {
                    _unitofwork.Company.Add(companyobj);

                }
                else
                {
                    _unitofwork.Company.Update(companyobj);
                }
                _unitofwork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyobj);
            }    
        }
        
        /*[HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product? obj = _unitofwork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitofwork.Product.Remove(obj);
            _unitofwork.Save();
            TempData["success"] = "Product Deleted successfully";
            return RedirectToAction("Index");
        }
        */

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitofwork.Company.Getall().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitofwork.Company.Get(u => u.Id == id);
            if(companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitofwork.Company.Remove(companyToBeDeleted);
            _unitofwork.Save();
            
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion
    }
}
