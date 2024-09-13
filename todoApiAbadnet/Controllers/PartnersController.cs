using Microsoft.AspNetCore.Mvc;
using todoApiAbadnet.Data;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PartnersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetPartnersList")]
        public IActionResult GetPartnersList()
        {
            var partnersList = _context.Partners
                .Select(p => new
                {
                    p.Name,
                    Image = "https://newabad.abadnet.com.sa" + p.Image
                })
                .ToList();

            return Ok(partnersList);
        }
    }

}
