using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using todoApiAbadnet.Data;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewsNowController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ViewsNowController(ApplicationDbContext context)
        {
            _context = context;
        }


        // API to check for an open offer and return the image with the base URL
        [HttpGet("CheckOpenOffer")]
        public IActionResult CheckOpenOffer()
        {
            // Retrieve the first record where IsOpen is true
            var openView = _context.ViewsNows.FirstOrDefault(v => v.IsOpen && v.DateClosed<=DateTime.Now);

            if (openView != null)
            {
                return Ok(new
                {
                    IsOpen = true,
                    Image = $"https://newabad.abadnet.com.sa/Admin/ViewsNowImage/{openView.Image}"
                });
            }
            else
            {
                return Ok(new
                {
                    IsOpen = false,
                    Image = ""
                });
            }
        }


        [HttpGet("CompareCourses")]
        public IActionResult CompareCourses(int courseNumber)
        {
            // Retrieve the first record where IsOpen is true
            var openView = _context.ViewsNows.FirstOrDefault(v => v.IsOpen);

            if (openView == null)
            {
                return Ok(new
                {
                    IsEligible = false,
                    Discount = 0
                });
            }

            // Compare the provided course number with NumberOfCourses
            if (courseNumber >= openView.NumberOfCourses)
            {
                return Ok(new
                {
                    IsEligible = true,
                    Discount = openView.PreDiscount
                });
            }
            else
            {
                return Ok(new
                {
                    IsEligible = false,
                    Discount = 0
                });
            }
        }


        [HttpPost("CheckDiscount")]
        public IActionResult CheckDiscount([FromForm] string discountCode, [FromForm] int numberOfCourses)
        {
            var openView = _context.ViewsNows.FirstOrDefault(v => v.IsOpen && v.DateClosed > DateTime.Now);
            var codeDiscount = _context.DiscountCodes.FirstOrDefault(v => v.IsActive && v.Code == discountCode && v.NumberOfCourse <= numberOfCourses);

            decimal codeDiscountAmount = 0;
            decimal offerDiscountAmount = 0;
            string message;

            if (codeDiscount != null)
            {
                // If there is a valid discount code
                codeDiscountAmount = codeDiscount.Discount;

                if (openView != null && openView.NumberOfCourses >= numberOfCourses)
                {
                    // If there is an open offer, apply the code discount and the open offer discount
                    offerDiscountAmount = openView.PreDiscount;
                    message = "يمكنك الاستفادة من عرض مفتوح وكود الخصم.";
                }
                else
                {
                    // Only code discount is applicable
                    message = "كود الخصم صحيح ! انت تستمع بالخصم الان.";
                }
            }
            else if (openView != null && openView.NumberOfCourses >= numberOfCourses)
            {
                // If there is an open offer but no valid discount code
                offerDiscountAmount = openView.PreDiscount;
                message = "كود الخصم غير صحيح، ولكن انت  مستفيد  من العرض الحالي.";
            }
            else
            {
                // No valid discount code and no open offer
                codeDiscountAmount = 0;
                offerDiscountAmount = 0;
                message = "كود الخصم غير صحيح.";
            }

            return Ok(new
            {
                IsDiscountApplicable = codeDiscountAmount > 0 || offerDiscountAmount > 0,
                Message = message,
                CodeDiscountPercentage = codeDiscountAmount,
                OfferDiscountPercentage = offerDiscountAmount
            });
        }


    }

}
