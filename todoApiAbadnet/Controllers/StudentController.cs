using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using todoApiAbadnet.Data;
using todoApiAbadnet.DTO;
using todoApiAbadnet.Models;
using WebApplicationAbad.Repository.RepositoryInterface;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork work;
        private readonly ApplicationDbContext context;
        [Obsolete]
        private readonly IHostingEnvironment host;

        [Obsolete]
        public StudentController(IUnitOfWork work, ApplicationDbContext context, IHostingEnvironment host)
        {
            this.work = work;
            this.context = context;
            this.host = host;
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
            return emailRegex.IsMatch(email);
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

        private static string ComputeSha256Hash(string rawData)
        {
            // إنشاء SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // حساب التجزئة
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // تحويل التجزئة إلى سلسلة نصية
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] StudentDto studentDto)
        {
            if (ModelState.IsValid)
            {
                // تحقق من تنسيق البريد الإلكتروني
                if (!IsValidEmail(studentDto.Email))
                {
                    return BadRequest(new { error = "تنسيق البريد الإلكتروني غير صالح." });
                }

                // تحقق من أن البريد الإلكتروني غير مستخدم بالفعل
                var existingStudentEmail = await context.Students.FirstOrDefaultAsync(s => s.Email == studentDto.Email);
                if (existingStudentEmail != null)
                {
                    return BadRequest(new { error = "البريد الإلكتروني مستخدم بالفعل." });
                }

                // تحقق من أن رقم الهاتف غير مستخدم بالفعل
                var existingStudentPhone = await context.Students.FirstOrDefaultAsync(s => s.Phone == studentDto.Phone);
                if (existingStudentPhone != null)
                {
                    return BadRequest(new { error = "رقم الهاتف مستخدم بالفعل." });
                }

                // تحقق من أن رقم الهوية غير مستخدم بالفعل
                var existingStudentIdNumber = await context.Students.FirstOrDefaultAsync(s => s.Idnumber == studentDto.Idnumber);
                if (existingStudentIdNumber != null)
                {
                    return BadRequest(new { error = "رقم الهوية مستخدم بالفعل." });
                }

                // تشفير كلمة المرور باستخدام SHA256
                string hashedPassword = ComputeSha256Hash(studentDto.Password);

                string emailBody = @"
<html>
<body style='direction: rtl; text-align: right;'>
    <h2>معهد أباد للتدريب المهني</h2>
    <p>مرحباً،</p>
    <p>نشكرك على تسجيلك في معهد أباد للتدريب المهني. نحن سعداء بانضمامك إلى مجتمعنا ونتطلع إلى مساعدتك في تحقيق أهدافك التعليمية والمهنية.</p>
    
    <h3>لماذا تختار معهد أباد؟</h3>
    <p>نحن معهد معتمد من المؤسسة العامة للتدريب المهني، ونقدم أفضل الدورات في المجالات التالية:</p>
    <ul>
        <li><strong>الشبكات (Networks)</strong></li>
        <li><strong>البرمجة (Programming)</strong></li>
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
        <strong>بريد إلكتروني:</strong> <a href='mailto:email@abad-institute.com'>email@abad-institute.com</a><br>
        <strong>هاتف:</strong> <a href='tel:123-456-7890'>123-456-7890</a>
    </p>

    <p>نتمنى لك تجربة تعليمية مثمرة وممتعة في معهد أباد.</p>

    <p>مع أطيب التحيات،<br>فريق معهد أباد للتدريب المهني</p>
</body>
</html>";
                // تحويل StudentDto إلى Student
                var student = new Student
                {
                    ArabicName = studentDto.ArabicName,
                    Idnumber = studentDto.Idnumber,
                    Email = studentDto.Email,
                    Phone = studentDto.Phone,
                    Gender = studentDto.Gender,
                    BirthDate = studentDto.BirthDate,
                    Nationality = studentDto.Nationality,
                    Password = hashedPassword, // تخزين كلمة المرور المشفرة
                    EducationsType = studentDto.EducationsType,
                    City = studentDto.City,
                    Token = Guid.NewGuid().ToString()
                };

                // إضافة الطالب الجديد إلى قاعدة البيانات
                context.Students.Add(student);
                await context.SaveChangesAsync();
                SendEmailAsync(student.Email, "معهد اباد للتدريب", emailBody);
                return Ok(new { message = "تم تسجيل الحساب بنجاح." });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            // تشفير كلمة المرور المدخلة لمقارنتها مع كلمة المرور المخزنة
            string hashedPassword = ComputeSha256Hash(loginRequest.Password);

            var student = await context.Students.FirstOrDefaultAsync(s => s.Email == loginRequest.Email && s.Password == hashedPassword);
            if (student == null)
            {
                return Unauthorized(new { message = "البريد الإلكتروني أو كلمة المرور غير صحيحة." });
            }

            // إنشاء كائن يحتوي على جميع بيانات الطالب
            var studentData = new
            {
                student.ArabicName,
                student.Idnumber,
                student.Email,
                student.Phone,
                student.Gender,
                student.BirthDate,
                student.Nationality,
                student.EducationsType,
                student.City,
                student.Token,
            };

            return Ok(studentData);
        }

        [HttpPost("validate-login")]
        public async Task<ActionResult<bool>> ValidateLogin([FromBody] LoginRequest loginRequest)
        {
            // تشفير كلمة المرور المدخلة لمقارنتها مع كلمة المرور المخزنة
            string hashedPassword = ComputeSha256Hash(loginRequest.Password);
            var studentExists = await context.Students
                .AnyAsync(s => s.Email == loginRequest.Email && s.Password == hashedPassword);

            return Ok(studentExists);
        }

        [HttpGet("checkToken")]
        public async Task<IActionResult> CheckToken(string token)
        {
            try
            {
                // البحث عن الطالب بناءً على التوكن
                var student = await context.Students.FirstOrDefaultAsync(s => s.Token == token);

                // استرجاع متغير true إذا وجد الطالب و false إذا لم يوجد
                bool studentExists = student != null;

                return Ok(new { exists = studentExists });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("checkEmail")]
        public async Task<IActionResult> CheckEmailStudent(string mail)
        {
            try
            {
                string email = mail.ToLower();
                var student = await context.Students.FirstOrDefaultAsync(b => b.Email == email);

                if (student != null)
                {
                    string tokenLink = $"https://abad-next.vercel.app/new-password/{student.Token}";
                    string mailBody = $@"
                <html>
                <body style='direction: rtl; text-align: right;'>
                    <p>عزيزي/عزيزتي {student.ArabicName},</p>
                    <p>نشكرك على اختيارك معهد آباد للتدريب المهني. لإعادة تعيين كلمة المرور الخاصة بك، يرجى النقر على الزر أدناه:</p>
                    <p style='text-align:center;'>
                        <a href='{tokenLink}' style='display:inline-block;padding:10px 20px;margin:10px;color:white;background-color:#007BFF;text-decoration:none;border-radius:5px;'>إعادة تعيين كلمة المرور</a>
                    </p>
                    <p>إذا لم تقم بطلب إعادة تعيين كلمة المرور، يرجى تجاهل هذا البريد الإلكتروني.</p>
                    <p>مع تحيات،<br>فريق معهد آباد للتدريب</p>
                </body>
                </html>";

                    SendEmailAsync(student.Email, "إعادة تعيين كلمة المرور", mailBody);

                    return Ok(new { success = true });
                }

                return Ok(new { success = false });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}  the InnerException : {ex.InnerException }");
            }
        }

        [HttpPost("newPassword")]
        public async Task<IActionResult> NewPassword([FromBody] NewPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var student = await context.Students.FirstOrDefaultAsync(n => n.Token == request.Token);
            if (student != null)
            {
                // تشفير كلمة المرور الجديدة
                student.Password = ComputeSha256Hash(request.Password);

                // تحديث التوكن
                student.Token = Guid.NewGuid().ToString();

                context.Students.Update(student);
                await context.SaveChangesAsync();

                return Ok(new { message = "تم تحديث كلمة المرور بنجاح." });
            }

            return NotFound(new { message = "الطالب غير موجود." });
        }

        [HttpPost("update/{token}")]
        public async Task<ActionResult> UpdateStudent(string token, [FromBody] StudentDto updateStudentDto)
        {
            if (ModelState.IsValid)
            {
                var student = await context.Students.FirstOrDefaultAsync(s => s.Token == token);
                if (student == null)
                {
                    return NotFound(new { error = "الطالب غير موجود." });
                }

                if (student.Email != updateStudentDto.Email)
                {
                    var existingStudentEmail = await context.Students.FirstOrDefaultAsync(s => s.Email == updateStudentDto.Email);
                    if (existingStudentEmail != null)
                    {
                        return BadRequest(new { error = "البريد الإلكتروني مستخدم بالفعل." });
                    }
                }

                if (student.Phone != updateStudentDto.Phone)
                {
                    var existingStudentPhone = await context.Students.FirstOrDefaultAsync(s => s.Phone == updateStudentDto.Phone);
                    if (existingStudentPhone != null)
                    {
                        return BadRequest(new { error = "رقم الهاتف مستخدم بالفعل." });
                    }
                }

                if (student.Idnumber != updateStudentDto.Idnumber)
                {
                    var existingStudentIdNumber = await context.Students.FirstOrDefaultAsync(s => s.Idnumber == updateStudentDto.Idnumber);
                    if (existingStudentIdNumber != null)
                    {
                        return BadRequest(new { error = "رقم الهوية مستخدم بالفعل." });
                    }
                }

                student.ArabicName = updateStudentDto.ArabicName;
                student.Idnumber = updateStudentDto.Idnumber;
                student.Email = updateStudentDto.Email;
                student.Phone = updateStudentDto.Phone;
                student.Gender = updateStudentDto.Gender;
                student.BirthDate = updateStudentDto.BirthDate;
                student.Nationality = updateStudentDto.Nationality;
                student.EducationsType = updateStudentDto.EducationsType;
                student.City = updateStudentDto.City;

                context.Students.Update(student);
                await context.SaveChangesAsync();

                return Ok(new { message = "تم تحديث بيانات الطالب بنجاح." });
            }
            return BadRequest(ModelState);
        }

        [HttpPost("update-password/{token}")]
        public async Task<ActionResult> UpdatePassword(string token, [FromBody] UpdatePasswordDto updatePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var student = await context.Students.FirstOrDefaultAsync(s => s.Token == token);
                if (student == null)
                {
                    return NotFound(new { error = "الطالب غير موجود." });
                }

                // تحقق من كلمة المرور القديمة
                if (student.Password != ComputeSha256Hash(updatePasswordDto.OldPassword))
                {
                    return BadRequest(new { error = "كلمة المرور القديمة غير صحيحة." });
                }

                // تحديث كلمة المرور
                student.Password = ComputeSha256Hash(updatePasswordDto.Password);
                student.Token = Guid.NewGuid().ToString();
                context.Students.Update(student);
                await context.SaveChangesAsync();

                // إرسال البريد الإلكتروني
                string tokenLink = $"https://abad-next.vercel.app/new-password/{student.Token}";
                string emailBody = $@"
<html>
<head>
    <link href='https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css' rel='stylesheet'>
</head>
<body style='direction: rtl; text-align: right;'>
    <div class='container' style='padding: 20px;'>
        <h2 class='text-center'>معهد أباد للتدريب المهني</h2>
        <p>عزيزي/عزيزتي {student.ArabicName},</p>
        <p>مرحباً،</p>
        <p>تم تحديث كلمة المرور الخاصة بك بنجاح. تفاصيل حسابك هي كالتالي:</p>
        <ul>
            <li><strong>البريد الإلكتروني:</strong> {student.Email}</li>
            <li><strong>كلمة المرور الجديدة:</strong> {updatePasswordDto.Password}</li>
        </ul>
        <p>إذا لم تطلب تغيير كلمة المرور، يمكنك إعادة تعيينها باستخدام الزر التالي:</p>
        <div class='text-center'>
            <a href='{tokenLink}' class='btn btn-primary'>إعادة تعيين كلمة المرور</a>
        </div>
        <h3>تواصل معنا</h3>
        <p>إذا كان لديك أي استفسار أو تحتاج إلى مساعدة، لا تتردد في التواصل معنا عبر البريد الإلكتروني أو الهاتف.</p>
        <p>
            <strong>بريد إلكتروني:</strong> <a href='mailto:email@abad-institute.com'>email@abad-institute.com</a><br>
            <strong>هاتف:</strong> <a href='tel:123-456-7890'>123-456-7890</a>
        </p>
        <p>مع أطيب التحيات،<br>فريق معهد أباد للتدريب المهني</p>
    </div>
</body>
</html>";

                SendEmailAsync(student.Email, "تحديث كلمة المرور", emailBody);

                return Ok(new { message = "تم تحديث كلمة المرور بنجاح." });
            }
            catch (Exception ex)
            {
                // سجل تفاصيل الاستثناء إذا لزم الأمر
                // LogException(ex); // تأكد من أن لديك آلية تسجيل الأخطاء

                // ارجع برسالة خطأ صديقة للمستخدم مع الرسالة الفعلية للاستثناء
                return StatusCode(500, new { error = "حدث خطأ أثناء تحديث كلمة المرور.", details = ex.Message });
            }
        }

        [HttpGet("getAllUsers")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllUsers()
        {
            var students = await context.Students
                .Where(s => !s.IsDelete)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Token = s.Token,
                    ArabicName = s.ArabicName,
                    Idnumber = s.Idnumber,
                    Email = s.Email,
                    Phone = s.Phone,
                    Gender = s.Gender,
                    BirthDate = s.BirthDate,
                    Countries = s.Countries,
                    Nationality = s.Nationality,
                    Password = s.Password,
                    ConfirmPassword = s.ConfirmPassword,
                    EducationsType = s.EducationsType,
                    City = s.City,
                    Nots = s.Nots,
                    Image = s.Image,
                    IsDelete = s.IsDelete,
                    IsBlock = s.IsBlock,
                    IsLocked = s.IsLocked,
                    Amount = s.Amount,
                    UserCode = s.UserCode,
                    CreatedDate = s.CreatedDate,
                    LastUpdateUserCode = s.LastUpdateUserCode,
                    LastUpdateDate = s.LastUpdateDate
                })
                .ToListAsync();

            return Ok(students);
        }


        //[HttpDelete("deleteAll")]
        //public async Task<IActionResult> DeleteAllStudents()
        //{
        //    try
        //    {
        //        var students = context.Students.ToList();
        //        if (students.Count == 0)
        //        {
        //            return NotFound(new { message = "لا يوجد طلاب لحذفهم." });
        //        }

        //        context.Students.RemoveRange(students);
        //        await context.SaveChangesAsync();

        //        return Ok(new { message = "تم حذف جميع الطلاب بنجاح." });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "حدث خطأ أثناء حذف الطلاب.", error = ex.Message });
        //    }
        //}

    }



}


