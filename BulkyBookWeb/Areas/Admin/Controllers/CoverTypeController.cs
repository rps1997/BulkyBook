using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers;
[Area("Admin")]

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> obj = _unitOfWork.CoverType.GetAll(); 
            return View(obj);
        }
        //GET
        public IActionResult Create()
        {
            return View();
        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);
                _unitOfWork.Save();
            TempData["success"] = "Cover type created!!!";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult Edit(int? id)
        {
            if(id== null)
            {
                return NotFound();
            }
            var coverTypefromDb = _unitOfWork.CoverType.GetFirstOrDefault(u=>u.Id==id);
            if (coverTypefromDb==null)
            {
                return NotFound();
            }
            return View(coverTypefromDb);

        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
            TempData["success"] = "Cover type updated!!!";
            return RedirectToAction("Index");
            }
            return View(obj);
        }
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coverTypefromDb = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (coverTypefromDb == null)
            {
                return NotFound();
            }
            return View(coverTypefromDb);

        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.CoverType.GetFirstOrDefault(i => i.Id == id);
                _unitOfWork.CoverType.Remove(obj);
                _unitOfWork.Save();
        TempData["success"] = "Cover type deleted!!!";
        return RedirectToAction("Index");
            
            //return View(obj);
        }
    }

