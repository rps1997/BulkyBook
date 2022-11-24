using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
//using BulkyBookWeb.Data;
//using BulkyBookWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers;
[Area("Admin")]
public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
    
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    
    
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList= _unitOfWork.Category.GetAll(/*includeProperties: "Category,CoverType"*/);
            return View(objCategoryList);
        }
        //GET
        public IActionResult Create()
        {
            
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "Name and Display order cannot be same");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Created!!!";
                return RedirectToAction("Index");
            }
            return View(obj);
             
        }

        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb= _unitOfWork.Category.GetFirstOrDefault(u=>u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(c => c.Id == id);
            if(categoryFromDb == null)
            {
                return NotFound();
            }
            
            return View(categoryFromDb);
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "Name and Display order cannot be same");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Updated!!!";
                return RedirectToAction("Index");
            }
            return View(obj);

        }
        public IActionResult Delete(int? id)
        {

            if (id == null || id == 0)
            {
                return NotFound();
            }
            //var categoryFromDb = _db.Categories.Find(id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (categoryFromDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryFromDbFirst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("CustomError", "Name and Display order cannot be same");
            //}
            var obj = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            if (obj != null)
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Deleted!!!";
                return RedirectToAction("Index");
            }
                
            
            return View(obj);

        }
        public IActionResult Details(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var catDet = _unitOfWork.Category.GetFirstOrDefault(c => c.Id == id);
            return View(catDet);
        }
    }

