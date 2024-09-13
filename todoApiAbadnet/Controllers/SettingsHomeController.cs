using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApiAbadnet.Data;


namespace WebApplicationAbad.Areas.Setting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetDataHomeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public GetDataHomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SettingsHome/getDataHome
        [HttpGet("getDataHome")]
        public async Task<IActionResult> GetDataHome()
        {
            var settingsHome = await _context.SettingsHomes.FirstOrDefaultAsync();
            if (settingsHome == null)
            {
                return BadRequest("No data found.");
            }

            // بناء مسار الفيديو
            if (!string.IsNullOrEmpty(settingsHome.lVideoURL))
            {
                string videoPath = $"https://newabad.abadnet.com.sa/uploads/{settingsHome.lVideoURL}";
                settingsHome.lVideoURL = videoPath;
            }

            return Ok(settingsHome);
        }



        // GET: api/Privacy
        [HttpGet("GetPrivacy")]
        public async Task<IActionResult> GetFirstPrivacyModel()
        {
            var privacyModel = await _context.PrivacyModels
                .FirstOrDefaultAsync();

            if (privacyModel == null)
            {
                return BadRequest("No data found.");
            }

            return Ok(privacyModel);
        }

    }
}
