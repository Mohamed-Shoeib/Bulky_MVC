using System.Diagnostics;
using System.Security.Claims;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Utiltiy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulkyweb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork unitOfWork;
        public HomeController(ILogger<HomeController> logger, IUnitOfWork _unitOfWork)
        {
            _logger = logger;
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Product> productList = unitOfWork.Product.GetAll(includeProperties: "Category,ProductImages");
            return View(productList);
        }

        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = unitOfWork.Product.Get(u => u.Id == productId, includeProperties: "Category,ProductImages"),
                Count = 1,
                ProductId = productId
            };
            return View(cart);
        }

        [HttpPost]
        public IActionResult Details(ShoppingCart shoppingcart)
        {
            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingcart.ApplicationUserId = userId;

            ShoppingCart cartFromDb = unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingcart.ProductId);
            if (cartFromDb != null)
            {
                cartFromDb.Count += shoppingcart.Count;
                unitOfWork.ShoppingCart.Update(cartFromDb);
                unitOfWork.Save();
            }
            else
            {
                unitOfWork.ShoppingCart.Add(shoppingcart);
                unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            TempData["success"] = "Cart Updated Successfully";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
