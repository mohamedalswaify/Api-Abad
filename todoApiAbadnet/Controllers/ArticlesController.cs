using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using todoApiAbadnet.Data;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public ArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Retrieves the latest 6 articles that are not deleted and not hidden.
        /// Formats the date using Arabic culture with Gregorian calendar and includes the read time.
        /// </summary>
        /// <returns>A list of the latest 6 formatted articles.</returns>
        [HttpGet("GetLatestArticles")]
        public IActionResult GetLatestArticles()
        {
            try
            {
                // إنشاء كائن CultureInfo للثقافة العربية واستخدام التقويم الميلادي
                var cultureInfo = new CultureInfo("ar-SA");
                cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();

                // استرجاع أحدث 6 مقالات التي ليست محذوفة وليست مخفية، وترتيبها حسب تاريخ النشر بترتيب تنازلي
                var articles = _context.Article
                    .Where(a => !a.IsDelete && !a.IsHide)
                    .OrderByDescending(a => a.PublishDate)
                    .Take(6) // استرجاع أحدث 6 مقالات فقط
                    .AsEnumerable() // الانتقال إلى التقييم من جهة العميل
                    .Select(a => new
                    {
                        Token = a.Token,
                        Title = a.Title,
                        PublishDate = a.PublishDate.ToString("dd MMMM yyyy", cultureInfo), // استخدام الثقافة العربية مع التقويم الميلادي
                        Image = $"https://newabad.abadnet.com.sa/Admin/ImageArticle/{a.IamgeArticle}",
                        CoursesTypeCode = a.CoursesType.ArabicName,
                        ReadTime = $"قراءة {a.ReadTime} دقائق"
                    })
                    .ToList();

                // تنسيق المقالات للاستجابة النهائية
                var formattedArticles = articles.Select(a => new
                {
                    a.Token,
                    a.Title,
                    FormattedDate = $"{a.PublishDate} | {a.ReadTime}",
                    a.Image,
                    a.CoursesTypeCode
                }).ToList();

                // إعادة قائمة المقالات المهيئة كاستجابة ناجحة
                return Ok(formattedArticles);
            }
            catch (Exception ex)
            {
                // معالجة الاستثناءات وتسجيلها إن لزم الأمر، ثم إعادة استجابة خطأ
                var errorResponse = new
                {
                    Message = "حدث خطا اثناء استرجاع المقالات",
                    Error = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }


        /// <summary>
        /// Retrieves a list of articles with specific fields formatted in a particular way.
        /// </summary>
        /// <returns>JSON formatted list of articles</returns>

        [HttpGet("GetArticles")]
        public IActionResult GetArticles()
        {
            try
            {
                var cultureInfo = new CultureInfo("ar-SA");
                cultureInfo.DateTimeFormat.Calendar = new GregorianCalendar();

                var articles = _context.Article
                    .Where(a => !a.IsDelete && !a.IsHide)
                    .OrderByDescending(a => a.PublishDate)
                    .AsEnumerable() // Move to client-side evaluation
                    .Select(a => new
                    {
                        Token = a.Token,
                        Title = a.Title,
                        PublishDate = a.PublishDate.ToString("dd MMMM yyyy", cultureInfo), // Use Arabic culture with Gregorian calendar
                        Image = $"https://newabad.abadnet.com.sa/Admin/ImageArticle/{a.IamgeArticle}",
                        CoursesTypeCode = a.CoursesType.ArabicName,
                        ReadTime = $"قراءة {a.ReadTime} دقائق"
                    })
                    .ToList();

                var formattedArticles = articles.Select(a => new
                {
                    a.Token,
                    a.Title,
                    FormattedDate = $"{a.PublishDate} | {a.ReadTime}",
                    a.Image,
                    a.CoursesTypeCode
                }).ToList();

                return Ok(formattedArticles);
            }
            catch (Exception ex)
            {
                // Log the exception (this can be adapted to your logging mechanism)
                // e.g., _logger.LogError(ex, "An error occurred while retrieving articles.");

                var errorResponse = new
                {
                    Message = "حدث خطا اثناء استرجاع المقالات",
                    Error = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }

        /// <summary>
        /// Retrieves detailed article information based on the provided token.
        /// </summary>
        /// <param name="token">The token of the article</param>
        /// <returns>JSON formatted article details</returns>
        [HttpGet("GetArticleDetails")]
        public IActionResult GetArticleDetails(string token)
        {
            try
            {
                var article = _context.Article
                    .Where(a => a.Token == token && !a.IsDelete && !a.IsHide)
                    .Select(a => new
                    {
                        Token = a.Token,
                        Content = a.Contetnt,
                        Author = a.Author,
                        AuthorJob = a.Authorjob,
                        AuthorImage = $"https://newabad.abadnet.com.sa/Admin/ImageAuthor/{a.IamgeAuthor}",
                        ArticleImage = $"https://newabad.abadnet.com.sa/Admin/ImageArticle/{a.IamgeArticle}"
                    })
                    .FirstOrDefault();

                if (article == null)
                {
                    return NotFound(new { Message = "المقال غير موجود" });
                }

                return Ok(article);
            }
            catch (Exception ex)
            {
                // Log the exception (this can be adapted to your logging mechanism)
                // e.g., _logger.LogError(ex, "An error occurred while retrieving the article details.");

                var errorResponse = new
                {
                    Message = "حدث خطا اثناء استرجاع تفاصيل المقال",
                    Error = ex.Message
                };

                return StatusCode(500, errorResponse);
            }
        }
    
    
    }

}
