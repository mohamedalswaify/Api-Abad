using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApiAbadnet.Data;
using todoApiAbadnet.Models;

namespace todoApiAbadnet.Controllers
{
	[EnableCors("AllowAll")]
	[Route("api/[controller]")]
    [ApiController]
    public class CampsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CampsController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("increment-visitors")]
        public async Task<IActionResult> IncrementVisitors()
        {
            var visitorCount = _context.VisitorCounts.FirstOrDefault();

            if (visitorCount == null)
            {
                visitorCount = new VisitorCount { Count = 1 };
                _context.VisitorCounts.Add(visitorCount);
            }
            else
            {
                visitorCount.Count += 1;
            }

            await _context.SaveChangesAsync();
            return Ok(visitorCount.Count);
        }

        [HttpGet("increment-Article")]
        public async Task<IActionResult> IncrementArticle()
        {
            var visitorCount = _context.ArticleViewCounts.FirstOrDefault();

            if (visitorCount == null)
            {
                visitorCount = new ArticleViewCount { Count = 1 };
                _context.ArticleViewCounts.Add(visitorCount);
            }
            else
            {
                visitorCount.Count += 1;
            }

            await _context.SaveChangesAsync();
            return Ok(visitorCount.Count);
        }



        [HttpGet("get-visitor-count")]
        public async Task<IActionResult> GetVisitorCount()
        {
            // بافتراض أن لديك صف واحد فقط في جدول الزيارات
            var visitor = await _context.VisitorCounts.FirstOrDefaultAsync();

            if (visitor == null)
            {
                return NotFound("No visitor data found.");
            }

            return Ok(visitor.Count);
        }
        [HttpPost]
        public async Task<IActionResult> RegisterCamp([FromBody] CampsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCamp = await _context.CampsModels
                .FirstOrDefaultAsync(c => c.PhoneNumber == model.PhoneNumber);

            if (existingCamp != null)
            {
                
                existingCamp.TypeCourse = model.TypeCourse;
                _context.SaveChanges();


                return Ok(new
                {
                    message = "تم حجز مقعدك بنجاح",
                    redirectUrl = existingCamp.TypeCourse == "حضوري"
                        ? "https://abadnet.com.sa/online/product/%d8%a7%d9%84%d9%85%d8%b3%d8%a7%d8%b1-%d8%a7%d9%84%d8%aa%d8%a3%d8%b3%d9%8a%d8%b3%d9%8a-%d9%84%d9%84%d8%a3%d9%85%d9%86-%d8%a7%d9%84%d8%b3%d9%8a%d8%a8%d8%b1%d8%a7%d9%86%d9%8a-cybersecurity-roadmap-level/"
                        : "https://abadnet.com.sa/online/product/%d8%a7%d9%84%d9%85%d8%b3%d8%a7%d8%b1-%d8%a7%d9%84%d8%aa%d8%a3%d8%b3%d9%8a%d8%b3%d9%8a-%d9%84%d9%84%d8%a3%d9%85%d9%86-%d8%a7%d9%84%d8%b3%d9%8a%d8%a8%d8%b1%d8%a7%d9%86%d9%8a-cybersecurity-roadmap-leve-2/"
                });
            }

            _context.CampsModels.Add(model);
            await _context.SaveChangesAsync();

            string redirectUrl = model.TypeCourse == "حضوري"
                ? "https://abadnet.com.sa/online/product/%d8%a7%d9%84%d9%85%d8%b3%d8%a7%d8%b1-%d8%a7%d9%84%d8%aa%d8%a3%d8%b3%d9%8a%d8%b3%d9%8a-%d9%84%d9%84%d8%a3%d9%85%d9%86-%d8%a7%d9%84%d8%b3%d9%8a%d8%a8%d8%b1%d8%a7%d9%86%d9%8a-cybersecurity-roadmap-level/"
                : "https://abadnet.com.sa/online/product/%d8%a7%d9%84%d9%85%d8%b3%d8%a7%d8%b1-%d8%a7%d9%84%d8%aa%d8%a3%d8%b3%d9%8a%d8%b3%d9%8a-%d9%84%d9%84%d8%a3%d9%85%d9%86-%d8%a7%d9%84%d8%b3%d9%8a%d8%a8%d8%b1%d8%a7%d9%86%d9%8a-cybersecurity-roadmap-leve-2/";

            return Ok(new
            {
                message = "تم حجز مقعدك بنجاح",
                redirectUrl
            });
        }
    }
}
