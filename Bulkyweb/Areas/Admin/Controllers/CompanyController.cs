using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utiltiy;
using Bulkyweb.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using System.Collections.Generic;
using System.Runtime.Intrinsics.Arm;

namespace Bulkyweb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly ICompanyRepository CompanyRepo;
        public readonly IUnitOfWork unitOfWork;

        public CompanyController(ICompanyRepository _CompanyRepo, IUnitOfWork _unitOfWork)
        {
            CompanyRepo = _CompanyRepo;
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> CompanyListobj = CompanyRepo.GetAll().ToList();
            return View(CompanyListobj);
        }
        public IActionResult Upsert(int? id)
        {
            // create
            if (id == null || id == 0)
            {
                return View(new Company());
            }
            // update
            else
            {
                Company companyObj = unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                {
                    CompanyRepo.Add(companyObj);
                }
                else
                {
                    CompanyRepo.Update(companyObj);
                }

                unitOfWork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            return View(companyObj);
        }

        #region API Call
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> CompanyListobj = CompanyRepo.GetAll().ToList();
            return Json(new { data = CompanyListobj });
        }

        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = CompanyRepo.Get(p => p.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false, Message = "Error While Deleteing" });
            }
            CompanyRepo.Remove(CompanyToBeDeleted);
            unitOfWork.Save();
            return Json(new { success = true, Message = "Delete Successfully" });
        }
        #endregion
    }
}
