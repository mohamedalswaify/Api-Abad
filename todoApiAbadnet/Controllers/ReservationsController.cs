using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using todoApiAbadnet.Data;
using todoApiAbadnet.Models;
using Newtonsoft.Json.Linq;
using todoApiAbadnet.DTO;
using System.Net.Mail;
using System.Text.RegularExpressions;
using static System.Net.WebRequestMethods;
using Microsoft.Identity.Client;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IConfiguration configuration;
      

        public ReservationsController(ApplicationDbContext context, HttpClient httpClient, IHttpClientFactory _httpClientFactory, IConfiguration _configuration)
        {

            this.context = context;
            _httpClient = httpClient;
            httpClientFactory = _httpClientFactory;
            configuration = _configuration;

        }


        /// <summary>
        /// Sends an email to the specified address.
        /// </summary>
        /// <param name="toEmail">Recipient email address.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="body">Email body.</param>
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



		/// <summary>
		/// Adds an offline course to a student's record based on the provided course and student tokens.
		/// </summary>
		/// <param name="TokenCourse">Token identifying the course.</param>
		/// <param name="TokenStudent">Token identifying the student.</param>
		/// <returns>Returns a success message or an error message if the operation fails.</returns>
		[HttpPost("add-offline-course")]
		public async Task<IActionResult> AddOfflineCourseToStudent(string TokenCourse, string TokenStudent)
		{
			try
			{
				// Verify the student's existence
				var student = await context.Students.FirstOrDefaultAsync(s => s.Token == TokenStudent);
				if (student == null)
				{
					return BadRequest(new { success = false, message = "الطالب غير متوفر." });
				}

				// Verify the course's existence
				var courseSchedule = await context.CoursesSchedulesses.FirstOrDefaultAsync(c => c.TokenNumber == TokenCourse);
				if (courseSchedule == null)
				{
					return BadRequest(new { success = false, message = "الدورة غير متوفرة." });
				}

				// Check if the reservation already exists
				var existingReservation = await context.CoursesReserveds
					.FirstOrDefaultAsync(r => r.StudentId == student.Id && r.CoursesSchedulessId == courseSchedule.Id);

				if (existingReservation != null)
				{
					return Ok(new { success = false, message = "أنت مسجل بالفعل في هذه الدورة." });
				}

				// Create a new reservation record
				var newReservation = new CoursesReserved
				{
					StudentId = student.Id,
					CoursesSchedulessId = courseSchedule.Id,
					StutusReserved = false,
					StutusPaidup = false,
					UserCode = student.Id.ToString(),
					Paidup = 0,
					Payment = 0,
					CreatedDate = DateTime.Now,
					LastUpdateDate = DateTime.Now,
					Tax = 0,
					Nots = "New course reservation"
				};

				// Add the new record to the database
				context.CoursesReserveds.Add(newReservation);
				await context.SaveChangesAsync();

				// Convert the course start date and time
				DateTime courseDateTime = courseSchedule.StartDate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.MinValue;
				string courseTime = courseSchedule.StartTime.ToString();
				string courseDate = courseSchedule.StartDate?.ToString("yyyy-MM-dd") ?? string.Empty;
				string courseDay = courseDateTime.DayOfWeek.ToString();

				string locationUrl = "https://www.google.com/maps/place/Abadnet+Institute+for+Training/@24.742645,46.765495,16z/data=!4m6!3m5!1s0x3e2f019f2e6aeb79:0x6e3ce4e617b5cb9c!8m2!3d24.7426454!4d46.7654953!16s%2Fg%2F1pxq5fd9x?hl=en-US&entry=ttu=معهد+أباد+للتدريب";

				string emailBody = $@"
<html>
<head>
    <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css'>
</head>
<body style='direction: rtl; text-align: right;'>
    <div class='container mt-5'>
        <div class='card'>
            <div class='card-header'>
                <h2>معهد أباد للتدريب المهني</h2>
            </div>
            <div class='card-body'>
                <p>مرحباً،</p>
                <p>نشكرك على تسجيلك في معهد أباد للتدريب المهني. نحن سعداء بانضمامك إلى مجتمعنا ونتطلع إلى مساعدتك في تحقيق أهدافك التعليمية والمهنية.</p>
                
                <h3>تفاصيل الدورة:</h3>
                <ul class='list-unstyled'>
                    <li><strong>اسم الدورة:</strong> {courseSchedule.CoursesData.HeaderAr}</li>
                    <li><strong>نوع الدورة:</strong> {courseSchedule.CoursesIsonline.ArabicName}</li>
                    <li><strong>تاريخ الدورة:</strong> {courseDate}</li>
                    <li><strong>اليوم:</strong> {courseDay}</li>
                    <li><strong>الوقت:</strong> {courseTime}</li>
                </ul>

                <h3>الموقع:</h3>
                <p>يمكنك العثور على موقع المعهد عبر <a href='{locationUrl}' class='btn btn-primary' target='_blank'>Google Maps</a>.</p>

                <h3>تواصل معنا</h3>
                <p>
                    <strong>بريد إلكتروني:</strong> <a href='mailto:email@abad-institute.com'>email@abad-institute.com</a><br>
                    <strong>هاتف:</strong> <a href='tel:123-456-7890'>123-456-7890</a><br>
                    <strong>واتساب:</strong> <a href='https://wa.me/1234567890' target='_blank'>تواصل عبر واتساب</a>
                </p>

                <p>نتمنى لك تجربة تعليمية مثمرة وممتعة في معهد أباد.</p>

                <p>مع أطيب التحيات،<br>فريق معهد أباد للتدريب المهني</p>
            </div>
        </div>
    </div>

    <script src='https://code.jquery.com/jquery-3.2.1.slim.min.js'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js'></script>
    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js'></script>
</body>
</html>";

				// Send the confirmation email
				await SendEmailAsync(student.Email, "معهد أباد للتدريب", emailBody);

				return Ok(new { success = true, message = "تم حجز مقعدك بنجاح."});
			}
			catch (Exception ex)
			{
				// Handle exceptions and return a suitable response
				return StatusCode(500, new { success = false, message = "حدث خطأ أثناء إضافة الدورة.", details = ex.Message });
			}
		}




		/// <summary>
		/// Adds a course to the student's basket.
		/// </summary>
		/// <param name="tokenCourse">Token identifying the course.</param>
		/// <param name="tokenStudent">Token identifying the student.</param>
		/// <returns>Returns the basket status or an error message if the operation fails.</returns>
		[HttpPost("AddToBasket")]
        public async Task<IActionResult> AddToBasket(string tokenCourse, string tokenStudent)
        {
            try
            {
                var student = context.Students.FirstOrDefault(s => s.Token == tokenStudent);
                if (student == null)
                {
                    return BadRequest(new { error = "Invalid student" });
                }

                var course = context.CoursesSchedulesses.FirstOrDefault(c => c.TokenNumber == tokenCourse);
                if (course == null)
                {
                    return BadRequest(new { error = "Invalid course" });
                }

                // Check if the course is already in the student's basket
                var existingBasketItem = context.CourseBaskets.FirstOrDefault(b => b.CoursesSchedulessId == course.Id && b.StudentId == student.Id);
                if (existingBasketItem != null)
                {
                    return BadRequest(new { error = "الدورة موجود بالفعل" });
                }

                var basketItem = new courseBasket
                {
                    CoursesSchedulessId = course.Id,
                    StudentId = student.Id
                };

                context.CourseBaskets.Add(basketItem);
                await context.SaveChangesAsync();

                return Ok(new { message = "تم اضافة الدورة الي السلة" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An unexpected error occurred: {ex.Message}" });
            }
        }


        /// <summary>
        /// Retrieves the basket items based on the student's token.
        /// </summary>
        /// <param name="tokenStudent">Token identifying the student.</param>
        /// <returns>Returns the basket items or an error message if the operation fails.</returns>

        // API لاسترجاع عناصر السلة بناءً على tokenStudent
        [HttpGet("GetBasketByToken")]
        public IActionResult GetBasketByToken(string tokenStudent)
        {
            try
            {
                var student = context.Students.FirstOrDefault(s => s.Token == tokenStudent);
                if (student == null)
                {
                    return BadRequest(new { error = "الطالب غير موجود" });
                }

                // Retrieve the data from the database without the date comparison
                var basketItemsQuery = context.CourseBaskets
                    .Where(b => b.StudentId == student.Id)
                    .Select(b => new
                    {
                        BasketToken = b.TokenNumber,
                        CourseName = b.CoursesScheduless.CoursesData.HeaderAr,
                        CourseImage = $"https://newabad.abadnet.com.sa/Admin/CoursesDataImage/{b.CoursesScheduless.CoursesData.Image}",
                        CoursePrice = b.CoursesScheduless.Cost,
                        CoursesSchedulestoken = b.CoursesScheduless.TokenNumber,
                        startDate = b.CoursesScheduless.StartDate
                    });
                var currentDate = DateOnly.FromDateTime(DateTime.Now);
                var maxDate = currentDate.AddDays(-6);
                var basketItems = basketItemsQuery.ToList()
                    .Where(b => b.startDate.Value>=maxDate)
                               
                    .ToList();

                return Ok(basketItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An unexpected error occurred: {ex.Message}" });
            }
        }



        /// <summary>
        /// Removes an item from the basket based on the basket token and deletes items that have passed the course start date by five days.
        /// </summary>
        /// <param name="tokenBasket">Token identifying the basket item.</param>
        /// <returns>Returns the removal status or an error message if the operation fails.</returns>

        // API لحذف عنصر من السلة بناءً على التوكن وحذف جميع العناصر التي تجاوزت موعد بدء الدورة بخمسة أيام
        [HttpDelete("RemoveFromBasket")]
        public async Task<IActionResult> RemoveFromBasket(string tokenBasket)
        {
            try
            {
                // حذف العنصر بناءً على توكن السلة
                var basketItem = context.CourseBaskets.FirstOrDefault(b => b.TokenNumber == tokenBasket);
                if (basketItem != null)
                {
                    context.CourseBaskets.Remove(basketItem);
                }

                //// الحصول على الوقت الحالي
                //var now = DateTime.Now;

                //// تحميل العناصر إلى الذاكرة وتحويل جزء من الاستعلام إلى الجانب العميل
                //var itemsToRemove = context.CourseBaskets
                //    .Include(b => b.CoursesScheduless) // تضمين بيانات الدورة
                //    .AsEnumerable() // التحويل إلى تقييم العميل
                //    .Where(b => b.CoursesScheduless != null && // التحقق من عدم وجود قيم null
                //                b.CoursesScheduless.StartDate.HasValue &&
                //                (b.CoursesScheduless.StartDate.Value.ToDateTime(TimeOnly.MinValue) - now).TotalDays > 5)
                //    .ToList();

                //if (itemsToRemove.Any())
                //{
                //    context.CourseBaskets.RemoveRange(itemsToRemove);
                //}

                await context.SaveChangesAsync();

                return Ok(new { message = "تم حذف الدورة بنجاح" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An unexpected error occurred: {ex.Message}" });
            }
        }


        /// <summary>
        /// Checks if a course exists based on the provided token.
        /// </summary>
        /// <param name="tokenCourse">The token of the course</param>
        /// <returns>JSON indicating whether the course exists</returns>
        [HttpGet("CheckCourse")]
        public async Task<IActionResult> CheckCourse(string tokenCourse)
        {
            try
            {
                // تحميل البيانات في الذاكرة لتحويل جزء من الاستعلام إلى الجانب العميل
                var courses = await context.CoursesSchedulesses
                    .Where(c => c.CoursesData.TokenNumber == tokenCourse && c.StartDate.HasValue && c.IsDelete==false)
                    .ToListAsync();

                // التحقق من الشرط على الجانب العميل
                var course = courses
                    .FirstOrDefault(c => (DateTime.Now - c.StartDate.Value.ToDateTime(new TimeOnly(0, 0))).TotalDays <= 5);

                // إعادة النتيجة بناءً على التحقق
                var response = new
                {
                    CourseExists = course != null,
                    CourseToken = course?.TokenNumber ?? tokenCourse
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = "خطا في بيانات الدورة حاول مره اخري",
                    Error = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }


        /// <summary>
        /// Get course details by token
        /// </summary>
        /// <param name="token">Token identifying the course.</param>
        /// <returns>Returns the course details.</returns>
        [HttpGet("GetCourseDetailsByToken")]
        public ActionResult GetCourseDetailsByToken(string token)
        {
            var course = context.CoursesData
                .Where(b => b.TokenNumber == token && !b.IsDelete && !b.IsHide)
                .Select(c => new CourseDto
                {
                    Token = c.TokenNumber,
                    CourseName = c.HeaderAr,
                    summaryAr = GetFirstNWords(c.SummaryAr, 13),
                    GoalsAr = c.GoalsAr,
                    TargetAr = c.TargetAr,
                    DetailsAr = c.DetailsAr,
                    TestAr = c.TestAr,
                    ImageUrl = $"https://newabad.abadnet.com.sa/Admin/CoursesDataImage/{c.Image}",
                    Price = c.Price,

                })
                .FirstOrDefault();

            if (course == null)
            {
                return BadRequest(new { error = "الدورة غير موجودة." });
            }

            return Ok(course);
        }
        //لاسترجاع 13 كلمه فقط 
        private static string GetFirstNWords(string text, int numberOfWords)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var words = text.Split(' ').Take(numberOfWords);
            return string.Join(" ", words) + (words.Count() == numberOfWords ? "..." : "");
        }


        /// <summary>
        /// Register a course request with provided details.
        /// </summary>
        /// <param name="request">Contains details like course token, user name, email, phone, city, notes, and language preference.</param>
        /// <returns>Returns a success message or an error message if the operation fails.</returns>

        [HttpPost("register-request")]
        public async Task<IActionResult> RegisterCourseRequest([FromBody] RequiredCourseRequest request)
        {
            try
            {
                // التحقق من صحة البيانات
                if (string.IsNullOrEmpty(request.TokenCourse) ||
                    string.IsNullOrEmpty(request.UsserName) ||
                    string.IsNullOrEmpty(request.UsserEmail) ||
                    string.IsNullOrEmpty(request.UserPhone))
                {
                    return BadRequest(new { error = "البيانات غير كاملة." });
                }

                // التحقق من صحة البريد الإلكتروني
                if (!IsValidEmail(request.UsserEmail))
                {
                    return BadRequest(new { error = "البريد الإلكتروني غير صحيح." });
                }

                // التحقق من وجود الدورة باستخدام توكن الكورس
                var courseData = await context.CoursesData
                    .Include(c => c.CoursesType)
                    .FirstOrDefaultAsync(c => c.TokenNumber == request.TokenCourse);

                if (courseData == null)
                {
                    return NotFound(new { error = "الدورة غير موجودة." });
                }

                //// التحقق من صحة البريد الإلكتروني باستخدام Hunter API
                //if (!await IsRealEmail(request.UsserEmail))
                //{
                //    return BadRequest(new { error = "البريد الإلكتروني غير حقيقي." });
                //}

                // إنشاء سجل جديد في RequiredCourse
                var newRequest = new RequiredCourse
                {
                    UsserName = request.UsserName,
                    UsserEmail = request.UsserEmail,
                    UserPhone = request.UserPhone,
                    UserCity = request.UserCity,
                    Nots = request.Nots,
                    IsAribic = false,
                    DateNw = DateTime.Now,
                    CoursesTypeCode = courseData.CoursesType.Code,
                    CoursesDataId = courseData.Id
                };

                context.RequiredCourse.Add(newRequest);
                await context.SaveChangesAsync();

                // إعداد البريد الإلكتروني
                string emailBody = $@"
<html>
<head>
    <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css'>
</head>
<body style='direction: rtl; text-align: right;'>
    <div class='container mt-5'>
        <div class='card'>
            <div class='card-header'>
                <h2>معهد أباد للتدريب المهني</h2>
            </div>
            <div class='card-body'>
                <p>مرحباً {request.UsserName},</p>
                <p>شكراً لك على تقديم طلبك للحصول على الدورة التدريبية. نحن قد استلمنا طلبك وسوف نتواصل معك قريباً لمزيد من التفاصيل.</p>
                
                <p>إذا كان لديك أي استفسار أو تحتاج إلى مساعدة إضافية، لا تتردد في الاتصال بنا.</p>
                
                <h3>تواصل معنا</h3>
                <p>
                    <strong>بريد إلكتروني:</strong> <a href='mailto:email@abad-institute.com'>email@abad-institute.com</a><br>
                    <strong>هاتف:</strong> <a href='tel:123-456-7890'>123-456-7890</a><br>
                    <strong>واتساب:</strong> <a href='https://wa.me/1234567890' target='_blank'>تواصل عبر واتساب</a>
                </p>

                <p>نتمنى لك تجربة تعليمية مثمرة وممتعة في معهد أباد.</p>

                <p>مع أطيب التحيات،<br>فريق معهد أباد للتدريب المهني</p>
            </div>
        </div>
    </div>

    <script src='https://code.jquery.com/jquery-3.2.1.slim.min.js'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js'></script>
    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js'></script>
</body>
</html>";

                // إرسال البريد الإلكتروني
             await   SendEmailAsync(request.UsserEmail, "تم استلام طلبك", emailBody);

                return Ok(new { message = "تم تسجيل طلبك بنجاح وسنقوم بالتواصل معك قريباً." });
            }
            catch (Exception ex)
            {
                // معالجة الاستثناءات وإعادة استجابة مناسبة
                return StatusCode(500, new { error = "حدث خطأ أثناء تسجيل طلبك.", details = ex.Message });
            }
        }

        // وظيفة للتحقق من صحة البريد الإلكتروني باستخدام تعبير عادي
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        // وظيفة للتحقق من صحة البريد الإلكتروني باستخدام Hunter API
        private async Task<bool> IsRealEmail(string email)
        {
            var client = httpClientFactory.CreateClient();
            var apiKey = configuration["Hunter:ApiKey"];
            var response = await client.GetAsync($"https://api.hunter.io/v2/email-verifier?email={email}&api_key={apiKey}");
            var content = await response.Content.ReadAsStringAsync();
            var jsonResponse = JObject.Parse(content);

            // التحقق من حالة البريد الإلكتروني
            var status = jsonResponse["data"]?["status"]?.ToString();
            return status == "valid";
        }


        /// <summary>
        /// Process payment without saving any data.
        /// </summary>
        /// <param name="tokenCourse">Token identifying the course.</param>
        /// <param name="TokenStudent">Token identifying the student.</param>
        /// <param name="describe">Optional description.</param>
        /// <param name="IsTamar">Flag indicating if TamarPay is used.</param>
        /// <returns>Returns the payment status or an error message if the operation fails.</returns>

        private CostCalculationResult CalculateCourseCostsAndDiscounts(List<string> tokenCoursesList,string DiscountCode)
        {
            var coursesWithCostList = new List<CourseWithCost>();
            decimal totalAmount = 0;
            decimal discountAmount = 0;

            var openView = context.ViewsNows.FirstOrDefault(v => v.IsOpen && v.DateClosed <= DateTime.Now && v.NumberOfCourses>= tokenCoursesList.Count);
            var codeDiscount = context.DiscountCodes.FirstOrDefault(v => v.IsActive && v.Code== DiscountCode && v.NumberOfCourse>= tokenCoursesList.Count);

            foreach (var courseToken in tokenCoursesList)
            {
                var course = context.CoursesSchedulesses.FirstOrDefault(b => b.TokenNumber == courseToken && !b.IsDelete);

                if (course == null)
                {
                    throw new Exception($"الدوره غير متوفره: {courseToken}");
                }

                // حساب تكلفة الدورة مع الضريبة
                decimal courseCostWithTax = course.Cost * 1.15m;

                // إضافة تفاصيل الدورة إلى القائمة
                var courseWithCost = new  CourseWithCost
                {
                    CourseId = course.Id,
                    CostWithTax = courseCostWithTax,
                    DISCOUNT = 0m
                };

                // إذا كان العرض المفتوح موجودًا وعدد الكورسات يساوي عدد القوائم، يتم تطبيق الخصم
         

                if (openView != null)
                {
                    // Apply discount based on openView
                    decimal discount = courseCostWithTax * (openView.PreDiscount / 100m);
                    courseWithCost.DISCOUNT = discount;
                    courseCostWithTax -= discount;
                    discountAmount += discount;
          
                }
                 if ( codeDiscount != null)
                {
                    // Apply discount based on codeDiscount only if no discount was applied earlier
                    decimal discount = courseCostWithTax * (codeDiscount.Discount / 100m);
                    courseWithCost.DISCOUNT = discount;
                    courseCostWithTax -= discount;
                    discountAmount += discount;
                }


                // إضافة تكلفة الدورة بعد الخصم إلى المجموع الكلي
                totalAmount += courseCostWithTax;

                coursesWithCostList.Add(courseWithCost);
            }

            return new CostCalculationResult
            {
                CoursesWithCostList = coursesWithCostList,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount
            };
        }



        private async Task<IActionResult> PreScoringAndCheckout(List<string> tokenCoursesList, string phone, string TokenStudent,string DiscountCode)
        {
            var GetStudent = context.Students.FirstOrDefault(c => c.Token == TokenStudent);
            if (GetStudent == null)
            {
                return BadRequest("Student not found.");
            }

            var resultOfData =CalculateCourseCostsAndDiscounts(tokenCoursesList, DiscountCode);

            string TokenPayment = Guid.NewGuid().ToString();

            PreScoring dataRequest = new PreScoring()
            {
                payment = new Payment()
                {
                    amount = resultOfData.TotalAmount.ToString("0.00"),
                    currency = "SAR",
                    description = $"list of courses: {resultOfData.CoursesWithCostList[0].CourseId}",
                    buyer = new Buyer
                    {
                        //phone = phone,
                        //email = GetStudent.Email,
                        phone = "500000001",
                        email = "card.success@tabby.ai",
                        name = GetStudent.ArabicName,
                        dob = GetStudent.BirthDate?.ToString("yyyy-MM-dd")
                    },
                    buyer_history = new BuyerHistory
                    {
                        registered_since = GetStudent.BirthDate?.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        loyalty_level = 0,
                        is_social_networks_connected = true,
                        is_phone_number_verified = true,
                        is_email_verified = true
                    },
                    order = new Order()
                    {
                        tax_amount = "0.00",
                        shipping_amount = "0.00",
                        discount_amount = "0.00",
                        updated_at = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        reference_id = Guid.NewGuid().ToString(),
                        items = new List<Item>()
                {
                    new Item()
                    {
                        title = $"list of courses: {resultOfData.CoursesWithCostList[0].CourseId}",
                        description = "Description",
                        quantity = 1,
                        unit_price = resultOfData.TotalAmount.ToString("0.00"),
                        discount_amount = "0.00",
                        reference_id = Guid.NewGuid().ToString(),
                        image_url = "http://example.com",
                        product_url = "http://example.com",
                        gender = "Male",
                        category = $"list of courses: {resultOfData.CoursesWithCostList[0].CourseId}",
                        color = "N/A",
                        product_material = "N/A",
                        size_type = "N/A",
                        size = "N/A",
                        brand = "N/A"
                    }
                }
                    },
                    order_history = new List<OrderHistory>()
            {
                new OrderHistory()
                {
                    purchased_at =  DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                    amount = resultOfData.TotalAmount.ToString("0.00"),
                    status = "new",
                    buyer = new Buyer()
                    {
                        phone = phone,
                        email = GetStudent.Email,
                        name = GetStudent.ArabicName,
                        dob = GetStudent.BirthDate?.ToString("yyyy-MM-dd")
                    },
                    shipping_address = new ShippingAddress()
                    {
                        city = GetStudent.City,
                        address = GetStudent.City,
                        zip = "00000"
                    }
                }
            },
                    shipping_address = new ShippingAddress()
                    {
                        city = GetStudent.City,
                        address = GetStudent.City,
                        zip = "00000"
                    },
                    meta = new Meta()
                    {
                        order_id = Guid.NewGuid().ToString(),
                        customer = GetStudent.Id.ToString()
                    }
                },
                lang = "en",
                merchant_code = "asdfwerdaazxv",
                merchant_urls = new MerchantUrls()
                {
                    success = $"https://myserverhost-001-site2.dtempurl.com/api/Tabby/success?TokenNumber={TokenPayment}",
                    failure = $"https://myserverhost-001-site2.dtempurl.com/api/Tabby/failure?TokenNumber={TokenPayment}",
                    cancel = $"https://myserverhost-001-site2.dtempurl.com/api/Tabby/failure?TokenNumber={TokenPayment}"
                }
            };

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.tabby.ai/api/v2/checkout");
            request.Headers.Add("Authorization", "Bearer pk_test_6c34f195-0503-4e2d-bd1e-2a80b4128389");
            var content = new StringContent(JsonConvert.SerializeObject(dataRequest), Encoding.UTF8, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return BadRequest($"Error: {response.StatusCode}, Details: {errorContent}");
            }

            var respo = await response.Content.ReadAsStringAsync();
            var respData = JsonConvert.DeserializeObject<Checkout>(respo);

            if (respData != null && respData.status == "created")
            {
                string URL = respData?.configuration?.available_products?.installments?.FirstOrDefault()?.web_url;

                if (!string.IsNullOrEmpty(URL))
                {
                    TabbyPayment tabbyPayment = new TabbyPayment
                    {
                        TokenPayment = TokenPayment,
                        checkout_url = URL,
                        StudentId = GetStudent.Id,
                        status = respData.status,
                        Created = DateTime.Now,
                        amountRequest = resultOfData.TotalAmount.ToString(),
                        isDelete = false,

                    };

                    context.TabbyPayments.Add(tabbyPayment);
                    context.SaveChanges();

                    // Retrieve the ID of the TabbyPayment after saving
                    int tabbyPaymentId = tabbyPayment.Id;

                    // Create a list to store course details associated with the TabbyPayment
                    List<TabbyPaymentCourse> tabbyPaymentCourses = new List<TabbyPaymentCourse>();

                    // Add course details to the list
                    foreach (var course in resultOfData.CoursesWithCostList)
                    {
                        var tabbyPaymentCourse = new TabbyPaymentCourse
                        {
                            TabbyPaymentId = tabbyPaymentId,
                            CoursesSchedulessId = course.CourseId,
                            CostWithTax = course.CostWithTax,
                            DISCOUNT = course.DISCOUNT,
                        };

                        tabbyPaymentCourses.Add(tabbyPaymentCourse);
                    }

                    // Add all course details to the context and save changes
                    context.TabbyPaymentCourses.AddRange(tabbyPaymentCourses);
                    await context.SaveChangesAsync();
                    return Ok(new { redirect_url = URL });
                   // return Redirect(URL);
                }
            }

            return BadRequest("Failed to create Tabby payment session.");
        }


        [HttpPost("PayWithoutSaveData")]
        public async Task<IActionResult> PayWithoutSaveData([FromForm] List<string> tokenCoursesList, [FromForm] string TokenStudent, [FromForm] bool IsTamar = false, [FromForm] bool IsTabby = false, [FromForm] string DiscountCode=null)
        {
            try
            {
                var GetStudent = context.Students.FirstOrDefault(c => c.Token == TokenStudent);
                if (GetStudent == null)
                {
                    return BadRequest(new { error = "الطالب غير متوفر" });
                }

                foreach (var courseToken in tokenCoursesList)
                {
                    var course = context.CoursesSchedulesses.FirstOrDefault(b => b.TokenNumber == courseToken && !b.IsDelete);
                    if (course == null)
                    {
                        return BadRequest(new { error = $"الدوره غير متوفره: {courseToken}" });
                    }
                }

                if (IsTamar)
                {
                    string PhoneNumberIsFaild = GetStudent.Phone.Replace("+", "");
                    if (PhoneNumberIsFaild.Length == 9)
                    {
                        return await CreateCheckout(TokenStudent, tokenCoursesList, PhoneNumberIsFaild, DiscountCode);
                    }
                    else
                    {
                        return BadRequest(new { error = "رقم الجوال غير متاح" });
                    }
                }
                else if (IsTabby)
                {
                    string PhoneNumberIsFaildTabby = GetStudent.Phone.Replace("+", "");
                    if (PhoneNumberIsFaildTabby.Length == 9)
                    {
                        return await PreScoringAndCheckout(tokenCoursesList, PhoneNumberIsFaildTabby, TokenStudent,DiscountCode);
                    }
                    else
                    {
                        return BadRequest(new { error = "رقم الجوال غير متاح" });
                    }
                }
                else
                {
                    var resultOfData = CalculateCourseCostsAndDiscounts(tokenCoursesList, DiscountCode);

                    var transaction = new Transaction
                    {
                        CartAmount = (float)resultOfData.TotalAmount,
                        CartCurrency = "SAR",
                        CartDescription = $"list of courses: {resultOfData.CoursesWithCostList[0].CourseId}",
                        CartId = $"CART#{Guid.NewGuid()}",
                        ProfileId = 52603,
                        ServerKey = "SMJNLZDJZK-HZWM9LBDBD-TRWL9RLR26",
                        TranClass = "ecom",
                        TranType = "sale",
                      // ReturnURL = $"https://s37kqk7b-7066.uks1.devtunnels.ms/api/Reservations/PayTabsCallback",
                       // ReturnURL = $"https://myserverhost-001-site2.dtempurl.com/api/Reservations/PayTabsCallback",
                        ReturnURL = $"https://kh.abadnet.com.sa/api/Reservations/PayTabsCallback",
                    };

                    string paymentUrl = "https://secure.paytabs.sa/payment/request";
                    string jsonBody = JsonConvert.SerializeObject(transaction);
                    var requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    using (var _httpClient = new HttpClient())
                    {
                        _httpClient.DefaultRequestHeaders.Add("authorization", transaction.ServerKey);

                        var response = await _httpClient.PostAsync(paymentUrl, requestContent);

                        if (!response.IsSuccessStatusCode)
                        {
                            var errorResponse = await response.Content.ReadAsStringAsync();
                            return BadRequest(new { error = $"Error from payment service: {errorResponse}" });
                        }

                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<PayTabsTransactionResponse>(responseContent);

                        if (result.IsSuccess && GetStudent.Id != 0)
                        {
                            PayTabsCallbackModel payTabsCallback = new PayTabsCallbackModel
                            {
                                AcquirerMessage = result.cart_description,
                                AcquirerRRN = result.tran_ref,
                                CartId = result.cart_id,
                                StudentId = GetStudent.Id,
                                CretedDate = DateTime.Now,
                                Amount = resultOfData.TotalAmount,
                                RespStatus = "GO TO PAYMENT"
                            };

                            context.PayTabsCallbackModel.Add(payTabsCallback);
                            await context.SaveChangesAsync();

                            int payTabsPaymentId = payTabsCallback.Id;

                            List<paytabsPaymentCourse> tamaraPaymentCourses = new List<paytabsPaymentCourse>();

                            foreach (var course in resultOfData.CoursesWithCostList)
                            {
                                var tamaraPaymentCourse = new paytabsPaymentCourse
                                {
                                    PayTabsCallbackModelId = payTabsPaymentId,
                                    CoursesSchedulessId = course.CourseId,
                                    CostWithTax = course.CostWithTax,
                                    DISCOUNT = course.DISCOUNT,
                                };

                                tamaraPaymentCourses.Add(tamaraPaymentCourse);
                            }

                            context.PaytabsPaymentCourses.AddRange(tamaraPaymentCourses);
                            context.SaveChanges();

                            return Ok(new { redirect_url = result.redirect_url });
                        }
                        else
                        {
                            return BadRequest(new { error = "خطأ أثناء الدفع، حاول مرة أخرى" });
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { error = $"HTTP Request Error: {ex.Message}" });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new { error = $"JSON Deserialization Error: {ex.Message}" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An unexpected error occurred: {ex.Message}" });
            }
        }






        //[HttpPost("PayWithoutSaveData")]
        //public async Task<IActionResult> PayWithoutSaveData(string tokenCourse, string TokenStudent, bool IsTamar = false)
        //{
        //    try
        //    {
        //        if (IsTamar)
        //        {
        //            var GetStudent = context.Students.FirstOrDefault(c => c.Token == TokenStudent);
        //            if (GetStudent == null)
        //            {
        //                return BadRequest(new { error = "الطالب غير متوفر" });
        //            }
        //            string PhoneNumberIsFaild = GetStudent.Phone.Replace("+", "");
        //            if (PhoneNumberIsFaild.Length == 9)
        //            {
        //                return await CreateCheckout(TokenStudent, tokenCourse, PhoneNumberIsFaild);
        //            }
        //            else
        //            {
        //                return BadRequest(new { error = "رقم الجوال غير متاح" });
        //            }



        //        }

        //        else
        //        {
        //            var GetStudent = context.Students.FirstOrDefault(c => c.Token == TokenStudent);
        //            if (GetStudent == null)
        //            {
        //                return BadRequest(new { error = "الطالب غير متوفر" });
        //            }

        //            var course = context.CoursesSchedulesses
        //                .FirstOrDefault(b => b.TokenNumber == tokenCourse && !b.IsDelete);

        //            if (course == null)
        //            {
        //                return BadRequest(new { error = "الدوره غير متوفره" });
        //            }

        //            float fullAmount = ((float)course.Cost * 1.15f);
        //            var transaction = new Transaction
        //            {
        //                CartAmount = fullAmount,
        //                CartCurrency = "SAR",
        //                CartDescription = course.CoursesData.HeaderAr,
        //                CartId = "CART#1001",
        //                ProfileId = 52603,
        //                ServerKey = "SMJNLZDJZK-HZWM9LBDBD-TRWL9RLR26",
        //                TranClass = "ecom",
        //                TranType = "sale",
        //                ReturnURL = $"https://kh.abadnet.com.sa/api/Reservations/PayTabsCallback?cousreToken={course.TokenNumber}&studentToken={GetStudent.Token}",
        //            };

        //            string paymentUrl = "https://secure.paytabs.sa/payment/request";
        //            string jsonBody = JsonConvert.SerializeObject(transaction);
        //            var requestContent = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        //            using (var _httpClient = new HttpClient())
        //            {
        //                _httpClient.DefaultRequestHeaders.Add("authorization", transaction.ServerKey);
        //                var response = await _httpClient.PostAsync(paymentUrl, requestContent);
        //                response.EnsureSuccessStatusCode();
        //                var responseContent = await response.Content.ReadAsStringAsync();
        //                var result = JsonConvert.DeserializeObject<PayTabsTransactionResponse>(responseContent);

        //                if (result.IsSuccess && GetStudent.Id != 0)
        //                {
        //                    var payTabsCallback = new PayTabsCallbackModel
        //                    {
        //                        AcquirerMessage = result.cart_description,
        //                        AcquirerRRN = result.tran_ref,
        //                        CartId = result.cart_id,
        //                        StudentId = GetStudent.Id,
        //                        coursesScheduleId = course.Id,
        //                        RespStatus = "GO TO PAYMENT"
        //                    };

        //                    context.PayTabsCallbackModel.Add(payTabsCallback);
        //                    await context.SaveChangesAsync();
        //                    return Ok(new { redirect_url = result.redirect_url });
        //                }
        //                else
        //                {
        //                    return BadRequest(new { error = " خطا اثناء الدفع حاول مرة اخري" });
        //                }
        //            }
        //        }
        //    }
        //    catch (HttpRequestException ex)
        //    {
        //        return StatusCode(500, new { error = $"HTTP Request Error: {ex.Message}" });
        //    }
        //    catch (JsonException ex)
        //    {
        //        return StatusCode(500, new { error = $"JSON Deserialization Error: {ex.Message}" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = $"An unexpected error occurred: {ex.Message}" });
        //    }
        //}



      

public class PayTabsCallbackModel2
    {
        [FromForm(Name = "acquirerMessage")]
        public string? AcquirerMessage { get; set; }

        [FromForm(Name = "acquirerRRN")]
        public string? AcquirerRRN { get; set; }

        [FromForm(Name = "cartId")]
        public string? CartId { get; set; }

        [FromForm(Name = "customerEmail")]
        public string? CustomerEmail { get; set; }

        [FromForm(Name = "respCode")]
        public string? RespCode { get; set; }

        [FromForm(Name = "respMessage")]
        public string? RespMessage { get; set; }

        [FromForm(Name = "respStatus")]
        public string? RespStatus { get; set; }

        [FromForm(Name = "token")]
        public string? Token { get; set; }

        [FromForm(Name = "tranRef")]
        public string? TranRef { get; set; }
    }


        [HttpPost("PayTabsCallback")]
        public async Task<IActionResult> PayTabsCallback([FromForm] PayTabsCallbackModel2 model)
        {
            try
            {
                var payment = await context.PayTabsCallbackModel
                    .FirstOrDefaultAsync(b =>
                        (b.CartId == model.CartId || b.AcquirerRRN == model.TranRef) &&
                        !string.IsNullOrEmpty(b.CartId) &&
                        !string.IsNullOrEmpty(b.AcquirerRRN));

                if (payment != null)
                {
                    // تحديث تفاصيل الدفع
                    payment.AcquirerRRN = model.TranRef;
                    payment.CartId = model.CartId;
                    payment.CustomerEmail = model.CustomerEmail;
                    payment.RespCode = model.RespCode;
                    payment.RespMessage = model.RespMessage;
                    payment.RespStatus = model.RespStatus;
                    payment.Token = model.Token;
                    payment.TranRef = model.TranRef;
                    payment.DateAfterPay = DateTime.Now;
                    payment.Nots = payment.CartId == model.CartId ? "الحصول علي مرجع العمليه" : "الحصول علي مرجع الكارت";
                    await context.SaveChangesAsync();
                }
                else
                {
                    // إنشاء إدخال جديد في PayTabsCallbackModel
                    PayTabsCallbackModel result = new PayTabsCallbackModel
                    {
                        AcquirerMessage = model.AcquirerMessage,
                        AcquirerRRN = model.AcquirerRRN,
                        CartId = model.CartId,
                        CustomerEmail = model.CustomerEmail,
                        RespCode = model.RespCode,
                        RespMessage = model.RespMessage,
                        RespStatus = model.RespStatus,
                        Token = model.Token,
                        TranRef = model.TranRef,
                        Nots = "لم يتم الحصول علي بيانات مرجع العمليه",
                        CretedDate = DateTime.Now
                    };
                    context.PayTabsCallbackModel.Add(result);
                    await context.SaveChangesAsync();

                    var status = result.RespStatus ?? "فشلت عمليه الدفع";
                    var errorMessage = result.StudentId.ToString() ?? "الطالب غير متوفر";

                    var encodedStatus = Uri.EscapeDataString(status);
                    var encodedError = Uri.EscapeDataString(errorMessage);

                    // التوجيه إلى PayController/GotoHome مع status و error
                    return RedirectToAction("GotoHome", "Pay", new { status = encodedStatus, error = encodedError });
                }

                // إذا كانت عملية الدفع ناجحة
                if (payment.RespStatus == "A")
                {
                    var coursesForPayment = await context.PaytabsPaymentCourses
                        .Where(pc => pc.PayTabsCallbackModelId == payment.Id)
                        .Include(pc => pc.CoursesScheduless) // التحميل الصريح للجدول المرتبط
                            .ThenInclude(cs => cs.CoursesData) // التحميل الصريح للـ CoursesData
                        .Include(pc => pc.CoursesScheduless)
                            .ThenInclude(cs => cs.CoursesIsonline) // التحميل الصريح لـ CoursesIsonline
                        .ToListAsync();

                    foreach (var course in coursesForPayment)
                    {
                        // التحقق من وجود الحجز بالفعل
                        var existingReservation = await context.CoursesReserveds
                            .FirstOrDefaultAsync(r => r.StudentId == payment.StudentId && r.CoursesSchedulessId == course.CoursesSchedulessId);

                        if (existingReservation == null)
                        {
                            // إذا لم يكن موجودًا، قم بإضافته
                            CoursesReserved reservation = new CoursesReserved
                            {
                                CoursesSchedulessId = course.CoursesSchedulessId,
                                StudentId = payment.StudentId.Value,
                                Paidup = course.CostWithTax,
                                StutusPaidup = true,
                                StutusReserved = true,
                                Tax = course.CostWithTax * 0.15m,
                                Payment = course.CostWithTax,
                                Discount = course.DISCOUNT,
                                Nots = "paytabs Pay",
                                CreatedDate = DateTime.Now,
                                TypePaidId = 2,
                                UserCode = payment.StudentId.ToString(),
                            };

                            context.CoursesReserveds.Add(reservation);
                        }
                        else
                        {
                            existingReservation.Paidup = course.CostWithTax;
                            existingReservation.StutusPaidup = true;
                            existingReservation.StutusReserved = true;
                            existingReservation.Tax = course.CostWithTax * 0.15m;
                            existingReservation.Payment = course.CostWithTax;
                            existingReservation.Discount = course.DISCOUNT;
                            existingReservation.Nots = "paytabs Pay";
                            existingReservation.CreatedDate = DateTime.Now;
                            existingReservation.TypePaidId = 2;
                            existingReservation.UserCode = payment.StudentId.ToString();

                            context.CoursesReserveds.Update(existingReservation);
                        }

                        // إرسال البريد الإلكتروني مع تفاصيل الدورة
                        DateOnly? dateOnly = course.CoursesScheduless.StartDate;
                        DateTime courseDateTime = dateOnly.HasValue ? dateOnly.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue;
                        string courseTime = course.CoursesScheduless.StartTime.ToString();
                        string courseDate = course.CoursesScheduless.StartDate.HasValue ? course.CoursesScheduless.StartDate.Value.ToString("yyyy-MM-dd") : string.Empty;
                        string courseDay = courseDateTime.DayOfWeek.ToString();
                        string locationUrl = "https://www.google.com/maps/place/Abadnet+Institute+for+Training/";

                        string emailBody = $@"
<html>
<head>
    <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css'>
</head>
<body style='direction: rtl; text-align: right;'>
    <div class='container mt-5'>
        <div class='card'>
            <div class='card-header'>
                <h2>معهد أباد للتدريب المهني</h2>
            </div>
            <div class='card-body'>
                <p>مرحباً،</p>
                <p>نشكرك على تسجيلك في معهد أباد للتدريب المهني. نحن سعداء بانضمامك إلى مجتمعنا ونتطلع إلى مساعدتك في تحقيق أهدافك التعليمية والمهنية.</p>
                
                <h3>تفاصيل الدورة:</h3>
                <ul class='list-unstyled'>
                    <li><strong>اسم الدورة:</strong> {course.CoursesScheduless.CoursesData.HeaderAr}</li>
                    <li><strong>نوع الدورة:</strong> {course.CoursesScheduless.CoursesIsonline.ArabicName}</li>
                    <li><strong>تاريخ الدورة:</strong> {courseDate}</li>
                    <li><strong>اليوم:</strong> {courseDay}</li>
                    <li><strong>الوقت:</strong> {courseTime}</li>
                    <li><strong>طريقة الدفع :</strong> فيزا </li>
                </ul>

                <h3>الموقع:</h3>
                <p>يمكنك العثور على موقع المعهد عبر <a href='{locationUrl}' class='btn btn-primary' target='_blank'>Google Maps</a>.</p>

                <h3>تواصل معنا</h3>
                <p>
                    <strong>بريد إلكتروني:</strong> <a href='mailto:email@abad-institute.com'>email@abad-institute.com</a><br>
                    <strong>هاتف:</strong> <a href='tel:123-456-7890'>123-456-7890</a><br>
                    <strong>واتساب:</strong> <a href='https://wa.me/1234567890' target='_blank'>تواصل عبر واتساب</a>
                </p>

                <p>نتمنى لك تجربة تعليمية مثمرة وممتعة في معهد أباد.</p>

                <p>مع أطيب التحيات،<br>فريق معهد أباد للتدريب المهني</p>
            </div>
        </div>
    </div>

    <script src='https://code.jquery.com/jquery-3.2.1.slim.min.js'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js'></script>
    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js'></script>
</body>
</html>";

                        await SendEmailAsync(payment.Students.Email, "معهد اباد للتدريب", emailBody);
                    }

                    await context.SaveChangesAsync();
                    return RedirectToAction("GotoHome", "Pay", new { status = "success" });
                }
                else
                {
                    return RedirectToAction("GotoHome", "Pay", new { status = "failed" });
                }
            }
            catch (Exception ex)
            {
                string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "LogPayment");
                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string logFileName = Path.Combine(logDirectory, $"PayTabsCallbackLog_{DateTime.Now:yyyyMMddHHmmss}.txt");
                await System.IO.File.WriteAllTextAsync(logFileName, $"Error: {ex.Message}\nStackTrace: {ex.StackTrace}");

                return RedirectToAction("GotoHome", "Pay", new { status = "error", error = ex.Message });
            }
        }




        /// <summary>
        /// Callback for PayTabs payment gateway.
        /// </summary>
        /// <param name="acquirerMessage">Message from the acquirer.</param>
        /// <param name="acquirerRRN">Acquirer RRN.</param>
        /// <param name="cartId">Cart ID.</param>
        /// <param name="customerEmail">Customer email.</param>
        /// <param name="respCode">Response code.</param>
        /// <param name="respMessage">Response message.</param>
        /// <param name="respStatus">Response status.</param>
        /// <param name="token">Token.</param>
        /// <param name="tranRef">Transaction reference.</param>
        /// <param name="cousreToken">Course token.</param>
        /// <param name="studentToken">Student token.</param>
        /// <returns>Returns the callback status or an error message if the operation fails.</returns>
        /// 

        //        [HttpPost("PayTabsCallback")]
        //        public async Task<IActionResult> PayTabsCallback([FromForm] string acquirerMessage = null!, [FromForm] string acquirerRRN = null!, [FromForm] string cartId = null!, [FromForm] string customerEmail = null!, [FromForm] string respCode = null!, [FromForm] string respMessage = null!, [FromForm] string respStatus = null!, [FromForm] string token = null!, [FromForm] string tranRef = null!)
        //        {
        //            try
        //            {
        //                var payment = await context.PayTabsCallbackModel
        //     .FirstOrDefaultAsync(b =>
        //         (b.CartId == cartId || b.AcquirerRRN == tranRef) &&
        //         !string.IsNullOrEmpty(b.CartId) &&
        //         !string.IsNullOrEmpty(b.AcquirerRRN));


        //                if (payment != null)
        //                {
        //                    // تحديث تفاصيل الدفع
        //                    payment.AcquirerRRN = tranRef;
        //                    payment.CartId = cartId;
        //                    payment.CustomerEmail = customerEmail;
        //                    payment.RespCode = respCode;
        //                    payment.RespMessage = respMessage;
        //                    payment.RespStatus = respStatus;
        //                    payment.Token = token;
        //                    payment.TranRef = tranRef;
        //                    payment.DateAfterPay = DateTime.Now;
        //                    payment.Nots = payment.CartId == cartId ? "الحصول علي مرجع العمليه" : "الحصول علي مرجع الكارت";
        //                    await context.SaveChangesAsync();
        //                }
        //                else
        //                {
        //                    // إنشاء إدخال جديد في PayTabsCallbackModel
        //                    PayTabsCallbackModel result = new PayTabsCallbackModel
        //                    {
        //                        AcquirerMessage = acquirerMessage,
        //                        AcquirerRRN = acquirerRRN,
        //                        CartId = cartId,
        //                        CustomerEmail = customerEmail,
        //                        RespCode = respCode,
        //                        RespMessage = respMessage,
        //                        RespStatus = respStatus,
        //                        Token = token,
        //                        TranRef = tranRef,
        //                        Nots = "لم يتم الحصول علي بيانات مرجع العمليه",
        //                        CretedDate = DateTime.Now
        //                    };
        //                    context.PayTabsCallbackModel.Add(result);
        //                    await context.SaveChangesAsync();

        //                    var status = result.RespStatus ?? "فشلت عمليه الدفع";
        //                    var errorMessage = result.StudentId.ToString() ?? "الطالب غير متوفر";

        //                    var encodedStatus = Uri.EscapeDataString(status);
        //                    var encodedError = Uri.EscapeDataString(errorMessage);

        //                    return Redirect($"https://abad-131.vercel.app/my-courses?status={encodedStatus}&error={encodedError}");




        //                }

        //                // إذا كانت عملية الدفع ناجحة
        //                if (payment.RespStatus == "A")
        //                {
        //                    var coursesForPayment = await context.PaytabsPaymentCourses
        //                        .Where(pc => pc.PayTabsCallbackModelId == payment.Id)
        //                        .Include(pc => pc.CoursesScheduless) // التحميل الصريح للجدول المرتبط
        //                            .ThenInclude(cs => cs.CoursesData) // التحميل الصريح للـ CoursesData
        //                        .Include(pc => pc.CoursesScheduless)
        //                            .ThenInclude(cs => cs.CoursesIsonline) // التحميل الصريح لـ CoursesIsonline
        //                        .ToListAsync();

        //                    foreach (var course in coursesForPayment)
        //                    {
        //                        // التحقق من وجود الحجز بالفعل
        //                        var existingReservation = await context.CoursesReserveds
        //                            .FirstOrDefaultAsync(r => r.StudentId == payment.StudentId && r.CoursesSchedulessId == course.CoursesSchedulessId);

        //                        if (existingReservation == null)
        //                        {
        //                            // إذا لم يكن موجودًا، قم بإضافته
        //                            CoursesReserved reservation = new CoursesReserved
        //                            {
        //                                CoursesSchedulessId = course.CoursesSchedulessId,
        //                                StudentId = payment.StudentId.Value,
        //                                Paidup = course.CostWithTax,
        //                                StutusPaidup = true,
        //                                StutusReserved = true,
        //                                Tax = course.CostWithTax * 0.15m,
        //                                Payment = course.CostWithTax,
        //                                Discount = course.DISCOUNT,
        //                                Nots = "paytabs Pay",
        //                                CreatedDate = DateTime.Now,
        //                                TypePaidId = 2,
        //                                UserCode = payment.StudentId.ToString(),
        //                            };

        //                            context.CoursesReserveds.Add(reservation);
        //                        }
        //                        else
        //                        {
        //                            existingReservation.Paidup = course.CostWithTax;
        //                            existingReservation.StutusPaidup = true;
        //                            existingReservation.StutusReserved = true;
        //                            existingReservation.Tax = course.CostWithTax * 0.15m;
        //                            existingReservation.Payment = course.CostWithTax;
        //                            existingReservation.Discount = course.DISCOUNT;
        //                            existingReservation.Nots = "paytabs Pay";
        //                            existingReservation.CreatedDate = DateTime.Now;
        //                            existingReservation.TypePaidId = 2;
        //                            existingReservation.UserCode = payment.StudentId.ToString();

        //                            context.CoursesReserveds.Update(existingReservation);
        //                        }

        //                        // إرسال البريد الإلكتروني مع تفاصيل الدورة
        //                        DateOnly? dateOnly = course.CoursesScheduless.StartDate;
        //                        DateTime courseDateTime = dateOnly.HasValue ? dateOnly.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue;
        //                        string courseTime = course.CoursesScheduless.StartTime.ToString();
        //                        string courseDate = course.CoursesScheduless.StartDate.HasValue ? course.CoursesScheduless.StartDate.Value.ToString("yyyy-MM-dd") : string.Empty;
        //                        string courseDay = courseDateTime.DayOfWeek.ToString();
        //                        string locationUrl = "https://www.google.com/maps/place/Abadnet+Institute+for+Training/";

        //                        string emailBody = $@"
        //<html>
        //<head>
        //    <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css'>
        //</head>
        //<body style='direction: rtl; text-align: right;'>
        //    <div class='container mt-5'>
        //        <div class='card'>
        //            <div class='card-header'>
        //                <h2>معهد أباد للتدريب المهني</h2>
        //            </div>
        //            <div class='card-body'>
        //                <p>مرحباً،</p>
        //                <p>نشكرك على تسجيلك في معهد أباد للتدريب المهني. نحن سعداء بانضمامك إلى مجتمعنا ونتطلع إلى مساعدتك في تحقيق أهدافك التعليمية والمهنية.</p>

        //                <h3>تفاصيل الدورة:</h3>
        //                <ul class='list-unstyled'>
        //                    <li><strong>اسم الدورة:</strong> {course.CoursesScheduless.CoursesData.HeaderAr}</li>
        //                    <li><strong>نوع الدورة:</strong> {course.CoursesScheduless.CoursesIsonline.ArabicName}</li>
        //                    <li><strong>تاريخ الدورة:</strong> {courseDate}</li>
        //                    <li><strong>اليوم:</strong> {courseDay}</li>
        //                    <li><strong>الوقت:</strong> {courseTime}</li>
        //                    <li><strong>طريقة الدفع :</strong> فيزا </li>
        //                </ul>

        //                <h3>الموقع:</h3>
        //                <p>يمكنك العثور على موقع المعهد عبر <a href='{locationUrl}' class='btn btn-primary' target='_blank'>Google Maps</a>.</p>

        //                <h3>تواصل معنا</h3>
        //                <p>
        //                    <strong>بريد إلكتروني:</strong> <a href='mailto:email@abad-institute.com'>email@abad-institute.com</a><br>
        //                    <strong>هاتف:</strong> <a href='tel:123-456-7890'>123-456-7890</a><br>
        //                    <strong>واتساب:</strong> <a href='https://wa.me/1234567890' target='_blank'>تواصل عبر واتساب</a>
        //                </p>

        //                <p>نتمنى لك تجربة تعليمية مثمرة وممتعة في معهد أباد.</p>

        //                <p>مع أطيب التحيات،<br>فريق معهد أباد للتدريب المهني</p>
        //            </div>
        //        </div>
        //    </div>

        //    <script src='https://code.jquery.com/jquery-3.2.1.slim.min.js'></script>
        //    <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js'></script>
        //    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js'></script>
        //</body>
        //</html>";

        //                        await SendEmailAsync(payment.Students.Email, "معهد اباد للتدريب", emailBody);
        //                    }

        //                    await context.SaveChangesAsync();
        //                    return Redirect($"https://abad-131.vercel.app/my-courses?status=success");
        //                }
        //                else
        //                {
        //                    return Redirect($"https://abad-131.vercel.app/my-courses?status=failed");
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                string logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "LogPayment");
        //                if (!Directory.Exists(logDirectory))
        //                {
        //                    Directory.CreateDirectory(logDirectory);
        //                }

        //                string logFileName = $"log_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid()}.txt";
        //                string logFilePath = Path.Combine(logDirectory, logFileName);

        //                string logContent = $"Date: {DateTime.Now}\n" +
        //                                    $"AcquirerMessage: {acquirerMessage}\n" +
        //                                    $"AcquirerRRN: {acquirerRRN}\n" +
        //                                    $"CartId: {cartId}\n" +
        //                                    $"CustomerEmail: {customerEmail}\n" +
        //                                    $"RespCode: {respCode}\n" +
        //                                    $"RespMessage: {respMessage}\n" +
        //                                    $"RespStatus: {respStatus}\n" +
        //                                    $"Token: {token}\n" +
        //                                    $"TranRef: {tranRef}\n" +
        //                                    $"Exception: {ex.ToString()}";

        //                await System.IO.File.WriteAllTextAsync(logFilePath, logContent);

        //                return Redirect($"https://abad-131.vercel.app/my-courses?status=failed");
        //            }
        //        }





        /// <summary>
        /// Create a checkout session for the specified student and course.
        /// </summary>
        /// <param name="TokenStudent">Token identifying the student.</param>
        /// <param name="tokenCourse">Token identifying the course.</param>
        /// <param name="phone">Student phone number.</param>
        /// <returns>Returns the checkout session details or an error message if the operation fails.</returns>

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCheckout(string TokenStudent, List<string> tokenCoursesList, string phone, string DiscountCode)
        {
            try
            {
                var GetStudent = context.Students.FirstOrDefault(c => c.Token == TokenStudent);
                if (GetStudent == null)
                {
                    return BadRequest(new { error = "الطالب غير موجود" });
                }

                // Use the helper method to calculate costs and discounts
                var result = CalculateCourseCostsAndDiscounts(tokenCoursesList, DiscountCode);



                var requestPayload = new
                {
                    total_amount = new { amount =result.TotalAmount, currency = "SAR" },
                    shipping_amount = new { amount = 0, currency = "SAR" },
                    tax_amount = new { amount = 0, currency = "SAR" },
                    order_reference_id = Guid.NewGuid().ToString(),
                    order_number = "S12356",
                    items = result.CoursesWithCostList.Select(course => new
                    {
                        name = course.CourseId.ToString(),
                        type = "Digital",
                        reference_id = course.CourseId,
                        sku = "SA-12436",
                        quantity = 1,
                        total_amount = new { amount = course.CostWithTax, currency = "SAR" }
                    }).ToArray(),
                    consumer = new
                    {
                       // email = GetStudent.Email,
                       email= "customer@email.com",
                        first_name = GetStudent.ArabicName,
                        last_name = GetStudent.ArabicName,
                        //phone_number = phone
                        phone_number = "566027755"
                    },
                    country_code = "SA",
                    description = "Payment for courses",
                    merchant_url = new
                    {
                        cancel = "https://myserverhost-001-site2.dtempurl.com/api/Reservations/approved",
                        failure = "https://myserverhost-001-site2.dtempurl.com/api/Reservations/approved",
                        success = "https://myserverhost-001-site2.dtempurl.com/api/Reservations/approved",
                        notification = "https://myserverhost-001-site2.dtempurl.com/api/Reservations/approved"
                    },
                    shipping_address = new
                    {
                        city = "Riyadh",
                        country_code = "SA",
                        first_name = GetStudent.ArabicName,
                        last_name = GetStudent.ArabicName,
                        line1 = "3764 Al Urubah Rd",
                        phone_number = phone,
                        region = "As Sulimaniyah"
                    }
                };

                var jsonPayload = JsonConvert.SerializeObject(requestPayload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // أضف رأس المصادقة هنا
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhY2NvdW50SWQiOiIwYmVmZDQ5Ni1jZjNhLTQ2NzktYTMwMy1iZmQ1ZWVlYWYxZjEiLCJ0eXBlIjoibWVyY2hhbnQiLCJzYWx0IjoiZjQ3M2NkZDJiMzU5NmQzZTUzMjBmM2ViMmJhMThlYmQiLCJyb2xlcyI6WyJST0xFX01FUkNIQU5UIl0sImlhdCI6MTcyMDM1OTk4MiwiaXNzIjoiVGFtYXJhIn0.w0xQpdmth2-U-YgrfuuXIqaQ3fnY4vDxLK0MjtZPZOYaTiT69r1-J8DRW3cNbmXMlPTLxynGyMgitm9FKbWkd53R5H90ckIe08HYnew0rUh-J0Ixrw-XbBeRsmrSAb12q5mD0NUa-UDLabpRd5luoHpGLLVSLhv1yGmGsWdGpf1oWfKCkLCZAiv8HrTZ41FN3Bxsr5G054ts4i97qt6n_pkimlz8UVYQa0tFM5qNLBJyu5q23mqRfYddp38RkMrJp9Fui8f8JAMe4n1ma4VWznd9xf-4bhvv5FLlL67qJvK4h73GFK4Ds2lalT9VGXjG4GZDdFRUXQwmC5din8_AvQ");

                var response = await _httpClient.PostAsync("https://api-sandbox.tamara.co/checkout", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var createCheckoutResponse = JsonConvert.DeserializeObject<CreateCheckoutResponse>(responseContent);

                    TamaraPayment tamara = new TamaraPayment
                    {
                        checkout_id = createCheckoutResponse.CheckoutId,
                        order_id = createCheckoutResponse.OrderId,
                        checkout_url = createCheckoutResponse.CheckoutUrl,
                        TotalAmountwithTaxAferDiscount = result.TotalAmount,
                        discountAmount =result.DiscountAmount,
                        StudentId = GetStudent.Id,
                        status = createCheckoutResponse.Status,
                        Created = DateTime.Now,
                        isDelete = false
                    };

                    // إضافة السجل الجديد إلى السياق
                    context.TamaraPayments.Add(tamara);
                    await context.SaveChangesAsync();

                    // استرجاع Id الكيان بعد الحفظ
                    int tamaraPaymentId = tamara.Id;

                    // إنشاء قائمة لتخزين تفاصيل الدورات
                    List<TamaraPaymentCourse> tamaraPaymentCourses = result.CoursesWithCostList.Select(course => new TamaraPaymentCourse
                    {
                        TamaraPaymentId = tamaraPaymentId,  // رقم العملية
                        CoursesSchedulessId = course.CourseId,
                        CostWithTax = course.CostWithTax,
                        DISCOUNT = course.DISCOUNT
                    }).ToList();

                    // إضافة كافة التفاصيل إلى السياق وحفظ التغييرات
                    context.TamaraPaymentCourses.AddRange(tamaraPaymentCourses);
                    await context.SaveChangesAsync();
                    return Ok(new { redirect_url = tamara.checkout_url });
                }

                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Approve the payment for the specified order.
        /// </summary>
        /// <param name="paymentStatus">Status of the payment.</param>
        /// <param name="orderId">ID of the order.</param>
        /// <returns>Returns the approval status or an error message if the operation fails.</returns>

        [HttpGet("approved")]
        public async Task<IActionResult> Approved(string paymentStatus, string orderId)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://api-sandbox.tamara.co/orders/{orderId}/authorise");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhY2NvdW50SWQiOiIwYmVmZDQ5Ni1jZjNhLTQ2NzktYTMwMy1iZmQ1ZWVlYWYxZjEiLCJ0eXBlIjoibWVyY2hhbnQiLCJzYWx0IjoiZjQ3M2NkZDJiMzU5NmQzZTUzMjBmM2ViMmJhMThlYmQiLCJyb2xlcyI6WyJST0xFX01FUkNIQU5UIl0sImlhdCI6MTcyMDM1OTk4MiwiaXNzIjoiVGFtYXJhIn0.w0xQpdmth2-U-YgrfuuXIqaQ3fnY4vDxLK0MjtZPZOYaTiT69r1-J8DRW3cNbmXMlPTLxynGyMgitm9FKbWkd53R5H90ckIe08HYnew0rUh-J0Ixrw-XbBeRsmrSAb12q5mD0NUa-UDLabpRd5luoHpGLLVSLhv1yGmGsWdGpf1oWfKCkLCZAiv8HrTZ41FN3Bxsr5G054ts4i97qt6n_pkimlz8UVYQa0tFM5qNLBJyu5q23mqRfYddp38RkMrJp9Fui8f8JAMe4n1ma4VWznd9xf-4bhvv5FLlL67qJvK4h73GFK4Ds2lalT9VGXjG4GZDdFRUXQwmC5din8_AvQ");
            request.Headers.Add("Cookie", "__cf_bm=eWFEtcwWHIRbu4E.U7YSJVJSpG3in4thTaBL7DC.0Hc-1720449877-1.0.1.1-onOPiKAzq45HpDqv4Sj8iCxpjyU.ehgSK.H0cFvax5xbMhn94FxlRjSNxRKTuGf3rr7yC3VcXtrfr4n_tdklKg");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return Redirect($"https://abad-next.vercel.app/my-courses?status=failed");
            }

            var details = await response.Content.ReadAsStringAsync();
            var authorizeOrderResponse = JsonConvert.DeserializeObject<AuthorizeOrderResponse>(details);

            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var authorizeOrderModel = new AuthorizeOrderModel
                    {
                        OrderId = authorizeOrderResponse.OrderId,
                        Status = authorizeOrderResponse.Status,
                        OrderExpiryTime = authorizeOrderResponse.OrderExpiryTime,
                        PaymentType = authorizeOrderResponse.PaymentType,
                        AutoCaptured = authorizeOrderResponse.AutoCaptured,
                        Amount = authorizeOrderResponse.AuthorizedAmount.Amount,
                        Currency = authorizeOrderResponse.AuthorizedAmount.Currency,
                        CaptureId = authorizeOrderResponse.CaptureId,
                        paymentStatus = paymentStatus
                    };

                    context.AuthorizeOrderModels.Add(authorizeOrderModel);
                    await context.SaveChangesAsync();

                    if (paymentStatus == "approved")
                    {
                        var tamara = context.TamaraPayments
                            .AsNoTracking()
                            .SingleOrDefault(b => b.order_id == orderId);

                        if (tamara != null)
                        {
                            var coursesForPayment = context.TamaraPaymentCourses
                                .AsNoTracking()
                                .Where(tc => tc.TamaraPaymentId == tamara.Id)
                                .ToList();

                            foreach (var course in coursesForPayment)
                            {
                                CoursesReserved coursesReserved = new CoursesReserved
                                {
                                    StutusReserved = true,
                                    StutusPaidup = true,
                                    Paidup = course.CostWithTax,
                                    Payment = course.CostWithTax,
                                    Discount = course.DISCOUNT,
                                    Tax = course.CostWithTax * 0.15m,
                                    Nots = "Tamara Pay",
                                    UserCode = tamara.StudentId.ToString(),
                                    CoursesSchedulessId = course.CoursesSchedulessId,
                                    StudentId = tamara.StudentId,
                                    TypePaidId = 3,
                                    CreatedDate = DateTime.Now
                                };

                                context.CoursesReserveds.Add(coursesReserved);

                                // تحويل التاريخ إلى DateTime
                                DateOnly? dateOnly = course.CoursesScheduless.StartDate;
                                DateTime courseDateTime = dateOnly.HasValue ? dateOnly.Value.ToDateTime(TimeOnly.MinValue) : DateTime.MinValue;
                                string courseTime = course.CoursesScheduless.StartTime.ToString();

                                string courseDate = course.CoursesScheduless.StartDate.HasValue ? course.CoursesScheduless.StartDate.Value.ToString("yyyy-MM-dd") : string.Empty; // التاريخ
                                string courseDay = courseDateTime.DayOfWeek.ToString(); // يوم الأسبوع
                                string locationUrl = "https://www.google.com/maps/place/Abadnet+Institute+for+Training/@24.742645,46.765495,16z/data=!4m6!3m5!1s0x3e2f019f2e6aeb79:0x6e3ce4e617b5cb9c!8m2!3d24.7426454!4d46.7654953!16s%2Fg%2F1pxq5fd9x?hl=en-US&entry=ttu=معهد+أباد+للتدريب"; // استبدل هذا بالعنوان الصحيح

                                string emailBody = $@"
<html>
<head>
    <link rel='stylesheet' href='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css'>
</head>
<body style='direction: rtl; text-align: right;'>
    <div class='container mt-5'>
        <div class='card'>
            <div class='card-header'>
                <h2>معهد أباد للتدريب المهني</h2>
            </div>
            <div class='card-body'>
                <p>مرحباً،</p>
                <p>نشكرك على تسجيلك في معهد أباد للتدريب المهني. نحن سعداء بانضمامك إلى مجتمعنا ونتطلع إلى مساعدتك في تحقيق أهدافك التعليمية والمهنية.</p>
                
                <h3>تفاصيل الدورة:</h3>
                <ul class='list-unstyled'>
                    <li><strong>اسم الدورة:</strong> {course.CoursesScheduless.CoursesData.HeaderAr}</li>
                    <li><strong>نوع الدورة:</strong> {course.CoursesScheduless.CoursesIsonline.ArabicName}</li>
                    <li><strong>تاريخ الدورة:</strong> {courseDate}</li>
                    <li><strong>اليوم:</strong> {courseDay}</li>
                    <li><strong>الوقت:</strong> {courseTime}</li>
                    <li><strong>'طريقة الدفع':</strong> تمارا Tamara</li>
                </ul>

                <h3>الموقع:</h3>
                <p>يمكنك العثور على موقع المعهد عبر <a href='{locationUrl}' class='btn btn-primary' target='_blank'>Google Maps</a>.</p>

                <h3>تواصل معنا</h3>
                <p>
                    <strong>بريد إلكتروني:</strong> <a href='mailto:email@abad-institute.com'>email@abad-institute.com</a><br>
                    <strong>هاتف:</strong> <a href='tel:123-456-7890'>123-456-7890</a><br>
                    <strong>واتساب:</strong> <a href='https://wa.me/1234567890' target='_blank'>تواصل عبر واتساب</a>
                </p>

                <p>نتمنى لك تجربة تعليمية مثمرة وممتعة في معهد أباد.</p>

                <p>مع أطيب التحيات،<br>فريق معهد أباد للتدريب المهني</p>
            </div>
        </div>
    </div>

    <script src='https://code.jquery.com/jquery-3.2.1.slim.min.js'></script>
    <script src='https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.11.0/umd/popper.min.js'></script>
    <script src='https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/js/bootstrap.min.js'></script>
</body>
</html>";

                                // إرسال البريد الإلكتروني
                             await   SendEmailAsync(tamara.Students.Email, "معهد اباد للتدريب", emailBody);


                            }

                            await context.SaveChangesAsync();
                            await transaction.CommitAsync();

                            return Redirect($"https://abad-next.vercel.app/my-courses?status=success");
                        }
                    }

                    await transaction.RollbackAsync();
                    return Redirect($"https://abad-next.vercel.app/my-courses?status=failed");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    // يمكنك تسجيل الاستثناء هنا إذا لزم الأمر
                    return Redirect($"https://abad-next.vercel.app/my-courses?status=failed");
                }
            }
        }

    }
}
