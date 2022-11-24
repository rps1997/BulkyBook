using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
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

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
        _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        
        //GET
        public IActionResult Upsert(int? id)
        {
        ProductVM productVM = new()
        {
            product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text= i.Name,
                Value=i.Id.ToString()
            }),
        };
        if (id== null || id==0)
            {
                //create
            //    ViewBag.CategoryList=CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
        else
            {
                //update
                productVM.product=_unitOfWork.Product.GetFirstOrDefault(u=>u.Id==id);
                return View(productVM);
            }
           

        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile?  file)
        {
            if (ModelState.IsValid)
            {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            if(file != null)
            {
                string fileName=Guid.NewGuid().ToString();
                var uplaods = Path.Combine(wwwRootPath, @"images\products\");
                var extension=Path.GetExtension(file.FileName);
                if(obj.product.ImageUrl!= null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                using (var fileStreams = new FileStream(Path.Combine(uplaods, fileName + extension), FileMode.Create)) 
                {
                    file.CopyTo(fileStreams);
                }
                obj.product.ImageUrl = @"\images\products\" + fileName + extension;
            }
            if (obj.product.Id == 0)
            {
                _unitOfWork.Product.Add(obj.product);
            }
            else
            {
                _unitOfWork.Product.Update(obj.product);
            }
                _unitOfWork.Save();
            if (obj.product.Id == 0)
            {
                TempData["success"] = "Product created!!!";
            }
            else
            {
                TempData["success"] = "Product updated!!!";
            }
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        
        
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        var productList=_unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
        return Json(new { data=productList });
    }

    //POST
    [HttpDelete]
    //[ValidateAntiForgeryToken]
    public IActionResult Delete(int? id)
    {

        var obj = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == id);
        if(obj == null)
        {
            return Json(new { success = false, message = "Error while selecting" });
        }
        string wwwRootPath = _hostEnvironment.WebRootPath;
        
        var oldImagePath = Path.Combine(wwwRootPath, obj.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath))
        {
            System.IO.File.Delete(oldImagePath);
        }
        
        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        //TempData["success"] = "Product deleted!!!";
        return Json(new { success = true, message = "Product deleted!!!" });
        //return RedirectToAction("Index");
        //return View(obj);
    }
    #endregion
}

