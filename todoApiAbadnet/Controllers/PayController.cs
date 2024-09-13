using Microsoft.AspNetCore.Mvc;

namespace todoApiAbadnet.Controllers
{
    public class PayController : Controller
    {


        // الأكشن GotoHome الذي سيتم استدعاؤه عند التوجيه من PayTabsCallback
        [HttpGet("GotoHome")]
        public IActionResult GotoHome(string status, string error)
        {
            // تعديل حالة status و error لتكون آمنة للاستخدام في الـ URL
            var encodedStatus = Uri.EscapeDataString(status ?? "failed");
            var encodedError = Uri.EscapeDataString(error ?? "Unknown Error");

            // إعادة التوجيه إلى الرابط المحدد مع الحالة والخطأ
            return Redirect($"https://abad-131.vercel.app/my-courses?status={encodedStatus}&error={encodedError}");
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
