using BulkyWebRazor_Temp.Data;
using BulkyWebRazor_Temp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BulkyWebRazor_Temp.Pages.Categories
{
    [BindProperties]
    public class deleteModel : PageModel
    {
        private readonly ApplicationDbContext db;
        public Category category { get; set; }
        public deleteModel(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            Category categoryFromDb = db.Categories.Find(category.Id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            else
            {
                db.Categories.Remove(categoryFromDb);
                db.SaveChanges();
                TempData["success"] = "Category Deleted Successfully";
                return RedirectToPage("Index");
            }
        }
    }
}
