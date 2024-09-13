using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using todoApiAbadnet.Data;
using todoApiAbadnet.DTO;
using WebApplicationAbad.Repository.RepositoryInterface;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IUnitOfWork work;
        private readonly ApplicationDbContext context;
        [Obsolete]
        private readonly IHostingEnvironment host;

        [Obsolete]
        public HomeController(IUnitOfWork work, ApplicationDbContext context, IHostingEnvironment host)
        {
            this.work = work;
            this.context = context;
            this.host = host;
        }

        // Get Courses in Home Page
        [HttpGet("latest")]
        public ActionResult GetLatestCourses()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var maxDate = currentDate.AddDays(-6);
            var latestCourses = context.CoursesSchedulesses
                .Where(b => !b.IsDelete && !b.IsHide && b.StartDate.Value >= maxDate && b.CoursesStatusId == 1)
                .OrderByDescending(c => c.StartDate)
                .ThenByDescending(c => c.StartTime)
                .Take(9)
                .Select(c => new CourseDto
                {
                    Token = c.TokenNumber,
                    CourseName = c.CoursesData.HeaderAr,
                    StartDate = c.StartDate!.Value, // Use ! to access Value from Nullable
                    IsOnline = c.CoursesIsonline.ArabicName,
                    Hadaf = c.IsHadaf ? "مدعومة من هدف" : null,
                    CategoryId = c.CoursesTypeCode,
                    CategoryName = c.CoursesType.ArabicName,
                    Price = c.Cost,
                    summaryAr = GetFirstNWords(c.CoursesData.SummaryAr, 13), // Display only the first 15 words of the course details,
                    ImageUrl = $"https://newabad.abadnet.com.sa/Admin/CoursesDataImage/{c.CoursesData.Image}",
                    FormattedTimeStart = GetPeriod(c.StartTime!.Value) + " " + FormatTime(c.StartTime!.Value), // Combine formatted time with period (صباحًا or مساءً)
                    FormattedTimeEnd = GetPeriod(c.EndTime!.Value) + " " + FormatTime(c.EndTime!.Value) // Combine formatted time with period (صباحًا or مساءً)
                })
                .ToList();

            return Ok(latestCourses);
        }

        //لاسترجاع 15 كلمه فقط 
        private static string GetFirstNWords(string text, int numberOfWords)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var words = text.Split(' ').Take(numberOfWords);
            return string.Join(" ", words) + (words.Count() == numberOfWords ? "..." : "");
        }

        // Helper method to format time to 12-hour format
        private static string FormatTime(TimeOnly time)
        {
            return time.ToString("hh:mm", CultureInfo.InvariantCulture);
        }

        // Helper method to get period (صباحًا or مساءً) based on time
        private static string GetPeriod(TimeOnly time)
        {
            return time.Hour < 12 ? "ص" : "م";
        }

        ////Get All Courses in Page Courses
        //[HttpGet("getAll")]
        //public ActionResult GetAllCourses()
        //{
        //    var currentDate = DateOnly.FromDateTime(DateTime.Now);
        //    var maxDate = currentDate.AddDays(-7);
        //    var allCourses = context.CoursesSchedulesses
        //        .Where(b => !b.IsDelete && !b.IsHide && b.StartDate.Value >= maxDate && b.CoursesStatusId == 1)
        //        .OrderByDescending(c => c.StartDate)
        //        .ThenByDescending(c => c.StartTime)
        //        .Select(c => new CourseDto
        //        {
        //            Token = c.TokenNumber,
        //            CourseName = c.CoursesData.HeaderAr,
        //            StartDate = c.StartDate!.Value, // Use ! to access Value from Nullable
        //            IsOnline = c.CoursesIsonline.ArabicName,
        //            Hadaf = c.IsHadaf ? "مدعومة من هدف" : null,
        //            CategoryId = c.CoursesTypeCode,
        //            CategoryName = c.CoursesType.ArabicName,
        //            Price = c.Cost,
        //            summaryAr = GetFirstNWords(c.CoursesData.SummaryAr, 13), // Display only the first 15 words of the course details
        //            ImageUrl = $"https://newabad.abadnet.com.sa/Admin/CoursesDataImage/{c.CoursesData.Image}",
        //            FormattedTimeStart = GetPeriod(c.StartTime!.Value) + " " + FormatTime(c.StartTime!.Value), // Combine formatted time with period (صباحًا or مساءً)
        //            FormattedTimeEnd = GetPeriod(c.EndTime!.Value) + " " + FormatTime(c.EndTime!.Value) // Combine formatted time with period (صباحًا or مساءً)
        //        })
        //        .ToList();

        //    return Ok(allCourses);
        //}


        // Get All Courses in Page Courses
        [HttpGet("getAll")]
        public ActionResult GetAllCourses()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var maxDate = currentDate.AddDays(-7);

            // Fetch data from the database
            var allCourses = context.CoursesSchedulesses
                .Where(b => !b.IsDelete && !b.IsHide && b.StartDate.Value >= maxDate && b.CoursesStatusId == 1)
                .OrderBy(c => c.StartDate)
                .ThenBy(c => c.StartTime)
                .Select(c => new
                {
                    c.StartDate,
                    Course = new CourseDto
                    {
                        Token = c.TokenNumber,
                        CourseName = c.CoursesData.HeaderAr,
                        StartDate = c.StartDate!.Value,
                        IsOnline = c.CoursesIsonline.ArabicName,
                        Hadaf = c.IsHadaf ? "مدعومة من هدف" : null,
                        CategoryId = c.CoursesTypeCode,
                        CategoryName = c.CoursesType.ArabicName,
                        Price = c.Cost,
                        summaryAr = GetFirstNWords(c.CoursesData.SummaryAr, 13),
                        ImageUrl = $"https://newabad.abadnet.com.sa/Admin/CoursesDataImage/{c.CoursesData.Image}",
                        FormattedTimeStart = GetPeriod(c.StartTime!.Value) + " " + FormatTime(c.StartTime!.Value),
                        FormattedTimeEnd = GetPeriod(c.EndTime!.Value) + " " + FormatTime(c.EndTime!.Value)
                    }
                })
                .AsEnumerable() // Switch to in-memory processing
                .GroupBy(c => c.StartDate!.Value.ToString("MMMM", new System.Globalization.CultureInfo("ar-AE"))) // Group by month in Arabic
                .ToDictionary(g => g.Key, g => new
                {
                    Month = g.Key,
                    Courses = g.Select(c => c.Course).ToList()
                });

            return Ok(allCourses);
        }



        private string FormatDate(DateTime date)
        {
            var culture = new CultureInfo("ar-SA");
            return date.ToString("dddd dd-MM-yyyy", culture);
        }

        private static string FormatCourseTime(TimeOnly startTime, TimeOnly endTime, DateOnly startDate)
        {
            string dayOfWeek = startDate.DayOfWeek switch
            {
                DayOfWeek.Friday => "الجمعة",
                DayOfWeek.Saturday => "السبت",
                DayOfWeek.Sunday => "الأحد",
                DayOfWeek.Monday => "الإثنين",
                DayOfWeek.Tuesday => "الثلاثاء",
                DayOfWeek.Wednesday => "الأربعاء",
                DayOfWeek.Thursday => "الخميس",
                _ => string.Empty
            };

            DateTime startDateTime = DateTime.Today.Add(startTime.ToTimeSpan());
            DateTime endDateTime = DateTime.Today.Add(endTime.ToTimeSpan());

            string formattedTime = $"{dayOfWeek} من الساعة {startDateTime.ToString("hh tt", new CultureInfo("ar-SA"))} إلى الساعة {endDateTime.ToString("hh tt", new CultureInfo("ar-SA"))}";

            if (startDate.DayOfWeek == DayOfWeek.Friday)
            {
                formattedTime = "الجمعة والسبت من الساعة ٦م الى الساعة ٩م";
            }
            else if (startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                formattedTime = "من الأحد إلى الأربعاء من الساعة ٦م الى الساعة ٩م";
            }

            return formattedTime;
        }

        // Get Course by Token
        [HttpGet("getByToken")]
        public ActionResult GetCourseByToken(string token)
        {
            var course = context.CoursesSchedulesses
                .Where(b => b.TokenNumber == token && !b.IsDelete && !b.IsHide && b.CoursesStatusId == 1)
                .Select(c => new CourseDto
                {
                    Token = c.TokenNumber,
                    CourseName = c.CoursesData.HeaderAr,
                    StartDate = c.StartDate!.Value,
                    IsOnline = c.CoursesIsonline.ArabicName,
                    Hadaf = c.IsHadaf ? "مدعومة من هدف" : null,
                    CategoryId = c.CoursesTypeCode,
                    CategoryName = c.CoursesType.ArabicName,
                    Price = c.Cost,
                    summaryAr = GetFirstNWords(c.CoursesData.SummaryAr, 13),
                    GoalsAr = c.CoursesData.GoalsAr,
                    TargetAr = c.CoursesData.TargetAr,
                    DetailsAr = c.CoursesData.DetailsAr,
                    TestAr = c.CoursesData.TestAr,
                    ImageUrl = $"https://newabad.abadnet.com.sa/Admin/CoursesDataImage/{c.CoursesData.Image}",
                    NumberOfweeks = c.NumberOfWeek,
                    NumberOfHours = c.NumberOfHourse,
                    FormattedTimeStart = FormatCourseTime(c.StartTime!.Value, c.EndTime!.Value, c.StartDate!.Value),
                    FormattedTimeEnd = FormatCourseTime(c.StartTime!.Value, c.EndTime!.Value, c.StartDate!.Value),
                    OpenCourses = context.CoursesSchedulesses
                        .Where(oc => oc.CoursesDataId == c.CoursesDataId && oc.CoursesStatusId == 1 && !oc.IsDelete && !oc.IsHide && oc.CoursesIsonlineId == c.CoursesIsonlineId)
                        .Select(oc => new CourseDto
                        {
                            Token = oc.TokenNumber,
                            CourseName = oc.CoursesData.HeaderAr,
                            StartDate = oc.StartDate!.Value,
                            FormattedTimeStart = FormatCourseTime(oc.StartTime!.Value, oc.EndTime!.Value, oc.StartDate!.Value),
                            FormattedTimeEnd = FormatCourseTime(oc.StartTime!.Value, oc.EndTime!.Value, oc.StartDate!.Value)
                        }).ToList() ?? new List<CourseDto>() // Ensure OpenCourses is not null
                })
                .FirstOrDefault();

            if (course == null)
            {
                return BadRequest(new { error = "الدوره غير موجودة." });
            }

            return Ok(course);
        }

        [HttpGet("basic/{userToken}")]
        public async Task<ActionResult> GetBasicCourseDetails(string userToken)
        {
            var student = await context.Students.FirstOrDefaultAsync(s => s.Token == userToken);
            if (student == null)
            {
                return BadRequest(new { error = "الطالب غير موجود." });
            }

            var courses = await context.CoursesSchedulesses
              .Where(c => context.CoursesReserveds.Any(cr => cr.StudentId == student.Id && cr.CoursesSchedulessId == c.Id && !cr.IsDelete &&
                  (c.CoursesTypeCode == 1 || cr.StutusReserved == true)))
              .Select(c => new
              {
                  Token = c.TokenNumber,
                  CourseName = c.CoursesData.HeaderAr,
                  IsOnline = c.CoursesIsonline.ArabicName,
                  TimeCourses = c.TimeCoures,
                  Schedule = $"{GetPeriod(c.StartTime!.Value)} {FormatTime(c.StartTime!.Value)} - {GetPeriod(c.EndTime!.Value)} {FormatTime(c.EndTime!.Value)}",
                  Summary = GetFirstNWords(c.CoursesData.SummaryAr, 20), // Adjust word count as needed
                  StutusReserved = context.CoursesReserveds
                                      .Where(cr => cr.StudentId == student.Id && cr.CoursesSchedulessId == c.Id && !cr.IsDelete)
                                      .Select(cr => cr.StutusReserved)
                                      .FirstOrDefault()
              })
              .ToListAsync(); // Use ToListAsync here

            var courseDetails = courses.Select(c => new
            {
                c.Token,
                c.CourseName,
                c.IsOnline,
                PeriodDays = GetPeriodDays(c.TimeCourses), // Apply logic here
                c.Schedule,
                c.Summary,
                c.StutusReserved
            }).ToList();
            return Ok(courseDetails);
        }

        private string GetPeriodDays(string timeCourses)
        {
            // Apply logic based on the value of timeCourses
            return timeCourses == "No WeekEnd" ? "يومياً" :
                   timeCourses == "WeekEnd" ? "الجمعة والسبت" :
                   "غير محدد";
        }

        [HttpGet("details/{courseToken}")]
        public async Task<ActionResult> GetCourseDetails(string courseToken)
        {
            var course = await context.CoursesSchedulesses
                .Include(c => c.CoursesData) // Include related entities as needed
                .Include(c => c.CoursesIsonline)
                .Include(c => c.SessionsRecords)
                .ThenInclude(sr => sr.sessionsDay)
                .ThenInclude(sd => sd.SessionsWeek)
                .FirstOrDefaultAsync(c => c.TokenNumber == courseToken && !c.IsDelete);

            if (course == null)
            {
                return NotFound(new { error = "الدورة غير موجودة." });
            }

            // Check if the course is online
            bool isOnline = course.CoursesIsonline.ArabicName == "أونلاين";

            var courseDetails = new
            {
                CourseName = course.CoursesData.HeaderAr,
                IsOnline = course.CoursesIsonline.ArabicName,
                Schedule = $"{GetPeriod(course.StartTime!.Value)} {FormatTime(course.StartTime!.Value)} - {GetPeriod(course.EndTime!.Value)} {FormatTime(course.EndTime!.Value)}",
                Description = course.CoursesData.SummaryAr,
                WhatsAppLink = course.LinkWhatsApp,
                DownloadLink = $"https://newabad.abadnet.com.sa/Admin/CoursesDataFiles/{course.CoursesData.TokenNumber}/{course.TokenNumber}/{course.CoursesData.FilesData}.Zip",
                ImageUrl = $"https://newabad.abadnet.com.sa/Admin/CoursesDataImage/{course.CoursesData.Image}",
                Sessions = isOnline ? course.SessionsRecords
                    .GroupBy(sr => sr.sessionsDay.SessionsWeek.Name) // Group by week
                    .Select(weekGroup => new
                    {
                        WeekName = weekGroup.Key,
                        Sessions = weekGroup.Select(sr => new
                        {
                            DayName = sr.sessionsDay.NameOfDay,
                            SessionUrl = sr.MeatingUrl,
                            IsReplacing = sr.IsReplcesing
                        }).ToList()
                    }).ToList() : null
            };

            return Ok(courseDetails);
        }
    }
}
