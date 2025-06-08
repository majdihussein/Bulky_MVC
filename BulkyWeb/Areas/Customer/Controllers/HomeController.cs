using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Bulky.Utility;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitofwork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitofwork = unitOfWork;
    }

    public IActionResult Index()
    {
        IEnumerable<Product> productList = _unitofwork.Product.Getall(includeProperties: "Category");
        return View(productList);
    }

    public IActionResult Details(int productId)
    {
        ShoppingCart cart = new ShoppingCart()
        {
            Product = _unitofwork.Product.Get(u => u.Id == productId, includeProperties: "Category"),
            Count = 1,
            ProductId = productId
        };
        return View(cart);
    }

    [HttpPost]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
        shoppingCart.ApplicationUserId = userId;

        ShoppingCart cartFromDb = _unitofwork.ShoppingCart.Get(u => u.ApplicationUserId == userId 
        && u.ProductId == shoppingCart.ProductId);

        if (cartFromDb != null)
        {
            // shopping cart exist
            cartFromDb.Count += shoppingCart.Count;
            _unitofwork.ShoppingCart.Update(cartFromDb);
            _unitofwork.Save();

        }
        else
        {
            _unitofwork.ShoppingCart.Add(shoppingCart);
            _unitofwork.Save();
            // for Session Management
            HttpContext.Session.SetInt32(SD.SessionCart, _unitofwork.ShoppingCart
                .Getall(u => u.ApplicationUserId == userId).Count());

        }
        TempData["success"] = "Shopping cart updated successfully!";


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
