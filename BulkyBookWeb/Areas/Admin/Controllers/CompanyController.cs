using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
//using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Collections.Generic;
//using System.Web.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering.SelectListItem;
//using SelectListItem = System.Web.Mvc.SelectListItem;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        
        }
        public IActionResult Index()
        {
            
            return View();
        }
        
        //GET
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id== null || id==0)
                {
                    return View(company);
                }
            else
                {
                    company=_unitOfWork.Company.GetFirstOrDefault(u=>u.Id==id);
                    return View(company);
                }
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company created!!!";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company updated!!!";
                }
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        
        
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var companyList=_unitOfWork.Company.GetAll();
        return Json(new { data=companyList });
    }

    //POST
    [HttpDelete]
    //[ValidateAntiForgeryToken]
    public IActionResult Delete(int? id)
    {

        var obj = _unitOfWork.Company.GetFirstOrDefault(i => i.Id == id);
        if(obj == null)
        {
            return Json(new { success = false, message = "Error while selecting" });
        }
        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        //TempData["success"] = "Product deleted!!!";
        return Json(new { success = true, message = "Company deleted!!!" });
        //return RedirectToAction("Index");
        //return View(obj);
    }
    #endregion
}

