using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels
{
    public class ProductReviewDetailsViewModel
    {

        public Product Product { get; set; }
        public IEnumerable<ProductReview> Reviews { get; set; }
        public double AverageRating { get; set; }
        public bool CanRate { get; set; }

    }
}
