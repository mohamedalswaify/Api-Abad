using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApiAbadnet.Data;
using todoApiAbadnet.Models;

namespace todoApiAbadnet.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContactUsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;
		private static DateTime lastRequestTime = DateTime.MinValue;

		public ContactUsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// POST: api/ContactUs
		[HttpPost]
		public async Task<IActionResult> SubmitContactUsForm([FromBody] ContactUs model)
		{
			if (ModelState.IsValid)
			{
				// التحقق من مرور 10 ثوانٍ على آخر طلب
				if ((DateTime.Now - lastRequestTime).TotalSeconds < 10)
				{
					return BadRequest(new { success = false, message = "لا يمكنك إرسال طلبك في الوقت الحالي." });
				}

				// جلب الطلبات التي تحتوي على نفس الاسم أو البريد الإلكتروني أو رقم الهاتف
				var recentRequests = await _context.ContactUs
					.Where(c => c.FullName == model.FullName || c.Email == model.Email || c.MobileNumber == model.MobileNumber)
					.ToListAsync();

				// التحقق إذا كان أي طلب حديث (أقل من 10 دقائق)
				if (recentRequests.Any(c => (DateTime.Now - c.CreatedDate).TotalMinutes < 10))
				{
					return BadRequest(new { success = false, message = "لا يمكنك إرسال طلب آخر بنفس الاسم أو البريد الإلكتروني أو رقم الهاتف إلا بعد مرور 10 دقائق." });
				}

				// تحديث الوقت الحالي كآخر وقت لطلب تم تقديمه
				lastRequestTime = DateTime.Now;

				// متابعة عملية الحفظ
				model.CreatedDate = DateTime.Now;
				_context.ContactUs.Add(model);
				await _context.SaveChangesAsync();

				return Ok(new { success = true, message = "تم إرسال رسالتك بنجاح." });
			}
			return BadRequest(new { success = false, message = "هناك مشكلة في تقديم طلبك." });
		}

		// GET: api/ContactUs
		[HttpGet]
		public async Task<IActionResult> GetContactUsForms()
		{
			var contactForms = await _context.ContactUs.ToListAsync();
			return Ok(contactForms);
		}
	}

}
