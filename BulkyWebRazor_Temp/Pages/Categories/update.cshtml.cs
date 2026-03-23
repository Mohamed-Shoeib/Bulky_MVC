using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class updateModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public Category category { get; set; }
        public updateModel(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            db.Categories.Update(category);
            db.SaveChanges();
            TempData["success"] = "Category UpdateD Successfully";
            return RedirectToPage("Index");
        }
    }
}
