using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulkyweb.DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

namespace Bulkyweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductRepository productRepo;
        private readonly IWebHostEnvironment webHostEnvironment;
        public readonly IUnitOfWork unitOfWork;

        public ProductController(IProductRepository _productRepo, IUnitOfWork _unitOfWork, IWebHostEnvironment _webHostEnvironment)
        {
            productRepo = _productRepo;
            webHostEnvironment = _webHostEnvironment;
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            List<Product> ProductListobj = productRepo.GetAll(includeProperties: "Category").ToList();
            return View(ProductListobj);
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM prdVm = new ProductVM()
            {
                CategoryList = unitOfWork.Category.GetAll()
                      .Select(u => new SelectListItem
                      {
                          Text = u.Name,
                          Value = u.Id.ToString()
                      }),
                product = new Product()
            };
            // create
            if (id == null || id == 0)
            {
                return View(prdVm);
            }
            // update
            else
            {
                prdVm.product = unitOfWork.Product.Get(u => u.Id == id);
                return View(prdVm);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM prdVm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, "Images", "Product");

                    if (!string.IsNullOrEmpty(prdVm.product.ImageURl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, prdVm.product.ImageURl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    prdVm.product.ImageURl = @"\Images\Product\" + fileName;
                }

                if (prdVm.product.Id == 0)
                {
                    productRepo.Add(prdVm.product);
                }
                else
                {
                    productRepo.Update(prdVm.product);
                }

                unitOfWork.Save();
                TempData["success"] = "Product Created Successfully";
                return RedirectToAction("Index");
            }

            prdVm.CategoryList = unitOfWork.Category.GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

            return View(prdVm);
        }

        #region API Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> ProductListobj = productRepo.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = ProductListobj });
        }

        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = productRepo.Get(p => p.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false, Message = "Error While Deleteing" });
            }
            var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, productToBeDeleted.ImageURl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            productRepo.Remove(productToBeDeleted);
            unitOfWork.Save();
            return Json(new { success = true, Message = "Delete Successfully" });
        }
        #endregion
    }
}
