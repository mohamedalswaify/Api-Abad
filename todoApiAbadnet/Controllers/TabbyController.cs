using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using todoApiAbadnet.Data;
using todoApiAbadnet.DTO;
using todoApiAbadnet.Models;


namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class TabbyController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<HomeController> _logger;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public TabbyController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            this.context = context;
          
        }

        private CostCalculationResult CalculateCourseCostsAndDiscounts(List<string> tokenCoursesList,string? DiscountCode)
        {
            var coursesWithCostList = new List<CourseWithCost>();
            decimal totalAmount = 0;
            decimal discountAmount = 0;

            var openView = context.ViewsNows.FirstOrDefault(v => v.IsOpen && v.NumberOfCourses >= tokenCoursesList.Count && v.DateClosed <= DateTime.Now);
            var codeDiscount = context.DiscountCodes.FirstOrDefault(v => v.IsActive && v.Code == DiscountCode && v.NumberOfCourse >= tokenCoursesList.Count);

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
                var courseWithCost = new CourseWithCost
                {
                    CourseId = course.Id,
                    CostWithTax = courseCostWithTax,
                    DISCOUNT = 0m
                };

                // إذا كان هناك عرض حالي موجودًا  يتم تطبيق الخصم
                if (openView != null )
                {
                    decimal discount = courseCostWithTax * (openView.PreDiscount / 100m);
                    courseWithCost.DISCOUNT = discount;
                    courseCostWithTax -= discount;
                    discountAmount += discount;
                }
                // إذا كان كود الخصم موجودًا  يتم تطبيق الخصم
                if (codeDiscount != null)
                {
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
                TotalAmount = Math.Round(totalAmount),
                DiscountAmount = Math.Round(discountAmount)
            };
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


        [HttpGet("PreScoringAndCheckout")]
        public async Task<IActionResult> PreScoringAndCheckout(List<string> tokenCoursesList, string phone, string TokenStudent ,string? DiscountCode)
        {
            var GetStudent = context.Students.FirstOrDefault(c => c.Token== TokenStudent);
            if (GetStudent == null)
            {
                return BadRequest("Student not found.");
            }

            var resultOfData = CalculateCourseCostsAndDiscounts(tokenCoursesList, DiscountCode);

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
                        phone = phone,
                        email = GetStudent.Email,
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
                        title = $"list of courses Token Payment is: {TokenPayment}",
                        description = "Description",
                        quantity =resultOfData.CoursesWithCostList.Count,
                        unit_price = resultOfData.TotalAmount.ToString("0.00"),
                        discount_amount = resultOfData.DiscountAmount.ToString( "0.00"),
                        reference_id = Guid.NewGuid().ToString(),
                        image_url = "http://example.com",
                        product_url = "http://example.com",
                        gender = "Male",
                        category = $"list of courses Token Payment is: {TokenPayment}",
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
                             CoursesSchedulessId= course.CourseId,     
                            CostWithTax = course.CostWithTax,
                            DISCOUNT = course.DISCOUNT,
                        };

                        tabbyPaymentCourses.Add(tabbyPaymentCourse);
                    }

                    // Add all course details to the context and save changes
                    context.TabbyPaymentCourses.AddRange(tabbyPaymentCourses);
                    await context.SaveChangesAsync();

                    return Redirect(URL);
                }
            }

            return BadRequest("Failed to create Tabby payment session.");
        }


        [HttpGet("success")]
        public async Task<IActionResult> success(string payment_id, string TokenNumber)
        {
            // استرجاع بيانات الدفع بناءً على payment_id
            var data = await RetrivePayment(payment_id);

            // البحث عن الدفع باستخدام TokenPayment
            var tabby = context.TabbyPayments.FirstOrDefault(b => b.TokenPayment == TokenNumber);

            if (tabby != null)
            {
                // تحديث تفاصيل الدفع
                tabby.paymentId = payment_id;
                tabby.status = data.status;
                tabby.UpdatedDate = DateTime.Now;
                tabby.amountResponse = data.amount;
                await context.SaveChangesAsync();

                // استرجاع الكورسات المرتبطة بعملية الدفع
                var coursesForPayment = await context.TabbyPaymentCourses
                    .AsNoTracking()
                    .Where(pc => pc.TabbyPaymentId == tabby.Id)
                    .ToListAsync();

                foreach (var course in coursesForPayment)
                {
                    // التحقق من الحجز السابق
                    var isResved = context.CoursesReserveds
                        .FirstOrDefault(b => b.StudentId == tabby.StudentId && b.CoursesSchedulessId == course.CoursesSchedulessId);

                    if (isResved != null)
                    {
                        // إذا كان الحجز موجوداً، يمكن تنفيذ تحديث أو تخطي العملية
                        // تعديل الحجز إذا لزم الأمر
                        //isResved.Paidup = Convert.ToDecimal(tabby.amountResponse);
                        isResved.Payment = Convert.ToDecimal(tabby.amountResponse);
                        isResved.Tax = Convert.ToDecimal(tabby.amountResponse) * 0.15m;
                        isResved.Discount = course.DISCOUNT;
                        isResved.StutusPaidup = true;
                        isResved.StutusReserved = true;
                        isResved.Nots = "الدفع بواسطه تابي";
                        isResved.TypePaidId = 4;
                        isResved.LastUpdateDate = DateTime.Now;
                    }
                    else
                    {
                        // إذا لم يكن الحجز موجوداً، يتم إنشاء حجز جديد
                        CoursesReserved newReservation = new CoursesReserved
                        {
                            CoursesSchedulessId = course.CoursesSchedulessId,
                            StudentId = tabby.StudentId,
                            CreatedDate = DateTime.Now,
                            Paidup = Convert.ToDecimal(tabby.amountResponse),
                            Payment = Convert.ToDecimal(tabby.amountResponse),
                            Tax = Convert.ToDecimal(tabby.amountResponse) * 0.15m,

                            IsCompany = false,
                            IsDelete = false,
                            StutusPaidup = true,
                            StutusReserved = true,
                            TypePaidId = 4,
                            UserCode = tabby.StudentId.ToString(),
                            Nots = "الدفع بواسطه تابي",
                        };

                        context.CoursesReserveds.Add(newReservation);


                    }
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
                    <li><strong>طريقة الدفع:</strong> تابي Tabby</li>
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
                   await SendEmailAsync(tabby.Students.Email, "معهد اباد للتدريب", emailBody);
                }

                await context.SaveChangesAsync();
                return Redirect($"https://abad-next.vercel.app/my-courses?status=success");
            }

            return Redirect($"https://abad-next.vercel.app/my-courses?status=failed");
        }

        [HttpGet("failure")]
        public async Task<IActionResult> failure(string payment_id, string TokenNumber)
        {

            // Update the order status in your database
            // Example: UpdateOrderStatus(orderId, status);

            //Your failure business logic


            var data = await RetrivePayment(payment_id);

            var tabby = context.TabbyPayments.FirstOrDefault(b => b.TokenPayment == TokenNumber);

            if (tabby != null)
            {
                tabby.paymentId = payment_id;
                tabby.status = data.status;
                tabby.UpdatedDate = DateTime.Now;
                tabby.amountResponse = data.amount;
                context.SaveChanges();
            }


            //var courseScueless = context.coursesSchedulesses.FirstOrDefault(b => b.Id == tabby.CoursesSchedulessId);
            //if (courseScueless != null)
            //{
            //    CoursesReserved CourseResved = new CoursesReserved
            //    {
            //        CoursesSchedulessId = tabby.CoursesSchedulessId,
            //        StudentId = tabby.StudentId,
            //        CreatedDate = DateTime.Now,
            //        Paidup = courseScueless.Cost,
            //        Payment = 0,
            //        Tax = Convert.ToDecimal(0.15),
            //        IsCompany = false,
            //        IsDelete = false,
            //        StutusPaidup = false,
            //        TypePaidId = 4,
            //        UserCode = tabby.StudentId.ToString(),
            //        Nots = "فشل الدفع بواسطه تابي",


            //    };
            //    context.CoursesReserveds.Add(CourseResved);
            //    context.SaveChanges();


            //}





            return RedirectToAction("FalidPayment", "Home");
        }

        private async Task<RetrivePaymentModel> RetrivePayment(string payment_id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, string.Format("https://api.tabby.ai/api/v2/payments/{0}", payment_id));
            request.Headers.Add("Authorization", "Bearer sk_test_b664af62-3c3a-4f88-b307-d28c99963916");
            //request.Headers.Add("Cookie", "_cfuvid=zulX11ndvvCQ389UD2VKPUCoZJ1C1I4bNK9ZFITbL9k-1723553804704-0.0.1.1-604800000");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            RetrivePaymentModel data = JsonConvert.DeserializeObject<RetrivePaymentModel>(await response.Content.ReadAsStringAsync());

            if (data.status == "AUTHORIZED")
            {
                //Call this only if you want to update the system order id in paymen portal;
                await UpdatePayment(data.id);

                var result = await CapturePayment(data.id, data.amount);

                // if Refund 
               // var refund = await RefundPayment(data.id, data.amount);
            }


            return data;
        }

        private async Task UpdatePayment(string payment_id)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Put, string.Format("https://api.tabby.ai/api/v2/payments/{0}", payment_id));
            request.Headers.Add("Authorization", "Bearer sk_test_b664af62-3c3a-4f88-b307-d28c99963916");
            //request.Headers.Add("Cookie", "_cfuvid=zulX11ndvvCQ389UD2VKPUCoZJ1C1I4bNK9ZFITbL9k-1723553804704-0.0.1.1-604800000");
            UpdatePaymentModel data = new UpdatePaymentModel()
            {
                order = new OrderUpdate
                {
                    reference_id = "Order Id"
                }
            };
            var content = new StringContent(JsonConvert.SerializeObject(data), null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private async Task<bool> CapturePayment(string payment_id, string amount)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, string.Format("https://api.tabby.ai/api/v2/payments/{0}/captures", payment_id));
                var data = new
                {
                    amount = amount
                };
                request.Headers.Add("Authorization", "Bearer sk_test_b664af62-3c3a-4f88-b307-d28c99963916");
                var content = new StringContent(JsonConvert.SerializeObject(data), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> RefundPayment(string payment_id, string amount)
        {
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, string.Format("https://api.tabby.ai/api/v2/payments/{0}/refunds", payment_id));
                var data = new
                {
                    amount = amount
                };
                request.Headers.Add("Authorization", "Bearer sk_test_b664af62-3c3a-4f88-b307-d28c99963916");
                var content = new StringContent(JsonConvert.SerializeObject(data), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                Console.WriteLine(await response.Content.ReadAsStringAsync());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



    }







}
