using System.Security.Claims;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ProductReviewController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductReviewController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


            // GET: عرض التقييمات وتفاصيل المنتج
            public IActionResult Details(int productId)
            {
                var product = _unitOfWork.Product
                    .Get(p => p.Id == productId, includeProperties: "ProductImages,Category");

                if (product == null)
                    return NotFound();

                var reviews = _unitOfWork.ProductReview
                    .Getall(r => r.ProductId == productId, includeProperties: "ApplicationUser")
                    .OrderByDescending(r => r.CreatedAt)
                    .ToList();

                double averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                bool canRate = false;

                if (!string.IsNullOrEmpty(userId))
                {
                    var hasPurchased = _unitOfWork.OrderHeader.Getall(o =>
                            o.ApplicationUserId == userId && o.OrderStatus == SD.StatusShipped)
                        .Any(order =>
                            _unitOfWork.OrderDetail.Getall(d =>
                                d.OrderHeaderId == order.Id && d.ProductId == productId).Any());

                    var alreadyRated = _unitOfWork.ProductReview
                        .Getall(r => r.ProductId == productId && r.ApplicationUserId == userId)
                        .Any();

                    canRate = hasPurchased && !alreadyRated;
                }

                var viewModel = new ProductReviewDetailsViewModel
                {
                    Product = product,
                    Reviews = reviews,
                    AverageRating = averageRating,
                    CanRate = canRate
                };

                return View(viewModel);
            }

            // POST: إضافة تقييم جديد
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult AddReview(ProductReview review)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var hasPurchased = _unitOfWork.OrderHeader.Getall(o =>
                        o.ApplicationUserId == userId && o.OrderStatus == SD.StatusShipped)
                    .Any(order =>
                        _unitOfWork.OrderDetail.Getall(d =>
                            d.OrderHeaderId == order.Id && d.ProductId == review.ProductId).Any());

                var alreadyRated = _unitOfWork.ProductReview
                    .Getall(r => r.ProductId == review.ProductId && r.ApplicationUserId == userId)
                    .Any();

                if (!hasPurchased || alreadyRated)
                    return Forbid();

                review.ApplicationUserId = userId;
                review.CreatedAt = DateTime.Now;

                _unitOfWork.ProductReview.Add(review);
                _unitOfWork.Save();

                return RedirectToAction("Details", new { productId = review.ProductId });
            }

        [AllowAnonymous]
        public IActionResult RatingsForHome()
        {
            var ratings = _unitOfWork.Product.Getall(includeProperties: "ProductReviews")
                .Select(p => new
                {
                    productId = p.Id, // ← لاحظ الحرف الصغير هنا
                    averageRating = p.ProductReviews.Any() ? p.ProductReviews.Average(r => r.Rating) : 0
                }).ToList();

            return Json(ratings);
        }


    }
}


