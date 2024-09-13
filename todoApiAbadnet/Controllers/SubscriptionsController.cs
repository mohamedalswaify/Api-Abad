using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using todoApiAbadnet.Data;
using todoApiAbadnet.Models;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public SubscriptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] Subscription subscription)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var existingSubscription = _context.Subscriptions
              .FirstOrDefault(s => s.Email == subscription.Email);

            if (existingSubscription != null)
            {
                return Conflict("البريد الكتروني مشترك بالفعل");
            }
            string emailBody = @"
    <html>
    <body style='direction: rtl; text-align: right;'>
        <h2>معهد أباد للتدريب المهني</h2>
        <p>مرحباً،</p>
        <p>نشكرك على تسجيل اشتراكك في معهد أباد للتدريب المهني. نحن سعداء بانضمامك إلى مجتمعنا ونتطلع إلى مساعدتك في تحقيق أهدافك التعليمية والمهنية.</p>
        
        <h3>لماذا تختار معهد أباد؟</h3>
        <p>نحن معهد معتمد من المؤسسة العامة للتدريب المهني، ونقدم أفضل الدورات في المجالات التالية:</p>
        <ul>
            <li><strong>البرمجة (Programming)</strong></li>
            <li><strong>الشبكات (Networks)</strong></li>
            <li><strong>أمن المعلومات (Information Security)</strong></li>
            <li><strong>جدار الحماية (Firewall)</strong></li>
            <li><strong>تحليل البيانات (Data Analysis)</strong></li>
            <li><strong>إدارة السيرفرات (Server Management)</strong></li>
            <li><strong>إدارة المشروعات (Project Management)</strong></li>
        </ul>

        <h3>عروض وخصومات خاصة</h3>
        <p>نحن نقدم مجموعة من العروض والخصومات الخاصة للطلاب الجدد والمستمرين. استفد من الفرص التالية:</p>
        <ul>
            <li>خصومات تصل إلى <strong>30%</strong> على الدورات المختارة.</li>
            <li>عروض خاصة عند التسجيل في أكثر من دورة.</li>
            <li>فرص للحصول على شهادات معتمدة مجاناً عند إكمال الدورات.</li>
        </ul>

        <h3>تواصل معنا</h3>
        <p>نحن هنا لمساعدتك في أي وقت. لا تتردد في التواصل معنا عبر البريد الإلكتروني أو الهاتف إذا كان لديك أي استفسار أو تحتاج إلى مساعدة.</p>
        <p>
            <strong>بريد إلكتروني:</strong> <a href='info@abad.com'>info@abad.com</a><br>
            <strong>هاتف:</strong> <a href='tel:123-456-7890'>123-456-7890</a>
        </p>

        <p>نتمنى لك تجربة تعليمية مثمرة وممتعة في معهد أباد.</p>

        <p>مع أطيب التحيات،<br>فريق معهد أباد للتدريب المهني</p>
    </body>
    </html>";
            SendEmailAsync(subscription.Email, "معهد اباد للتدريب", emailBody);
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();

            return Ok("تم الاشتراك بنجاح");
        }


        private void SendEmail(string toEmail, string subject, string body)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress("Starshieldsa1992@gmail.com");
                mailMessage.To.Add(toEmail);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;

                using (var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 25,
                    Credentials = new System.Net.NetworkCredential("Starshieldsa1992@gmail.com", "vxwmpphzuhypxdrm"),
                    EnableSsl = true,
                    UseDefaultCredentials = false
                })
                {
                    smtpClient.Send(mailMessage);
                }
            }
        }


        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using (var mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("Starshieldsa1992@gmail.com");
                    mailMessage.To.Add(toEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;

                    using (var smtpClient = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        Credentials = new System.Net.NetworkCredential("Starshieldsa1992@gmail.com", "vxwmpphzuhypxdrm"),
                        EnableSsl = true,
                        UseDefaultCredentials = false
                    })
                    {
                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }
            }
            catch
            {
                // No action taken in the catch block
            }
        }

    }
}
