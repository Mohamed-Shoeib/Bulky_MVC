using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulkyweb.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;

namespace Bulkyweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepo;
        private readonly IUnitOfWork unitOfWork;
        public CategoryController(ICategoryRepository _categoryRepo, IUnitOfWork _unitOfWork)
        {
            categoryRepo = _categoryRepo;
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category catObj)
        {
            if (catObj.Name == catObj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid)
            {
                categoryRepo.Add(catObj);
                unitOfWork.Save();
                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }
            else
                return View(catObj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = categoryRepo.Get(c =>c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category catFromReq)
        {
            if (ModelState.IsValid)
            {
                categoryRepo.Update(catFromReq);
                unitOfWork.Save();
                TempData["success"] = "Category Updated Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = categoryRepo.Get(c => c.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Deletepost(int? id)
        {
            Category catObj = categoryRepo.Get(c => c.Id == id);
            if (catObj == null)
            {
                return NotFound();
            }
            categoryRepo.Remove(catObj);
            unitOfWork.Save();
            TempData["success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}
