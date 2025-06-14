using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Bulky.Utility;
using Bulky.Models.ViewModels;

namespace BulkyWeb.Areas.Customer.Controllers;

[Area("Customer")]
public class ChartController : Controller
{
    private readonly IUnitOfWork _unitofwork;

    public ChartController(IUnitOfWork unitOfWork)
    {
        _unitofwork = unitOfWork;
    }

    public IActionResult Index()
    {
        var chartVM = new ChartVM()
        {
            TotalUser = _unitofwork.ApplicationUser.Getall().Count(),
            TotalOrder = _unitofwork.OrderHeader.Getall().Count(),
            TotalProduct = _unitofwork.Product.Getall().Count()
        };

        return View(chartVM);
    }


}
