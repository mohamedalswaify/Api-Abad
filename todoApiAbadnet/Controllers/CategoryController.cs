using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApiAbadnet.Data;
using todoApiAbadnet.DTO;
using WebApplicationAbad.Repository.RepositoryInterface;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork work;
        private readonly ApplicationDbContext context;

        [Obsolete]
        public CategoryController(IUnitOfWork work, ApplicationDbContext context)
        {
            this.work = work;
            this.context = context;
           
        }


        /// <summary>
        /// Retrieves a list of course types from the database.
        /// </summary>
        /// <returns>A list of CoursesTypeDto objects containing the course types.</returns>
        /// <response code="200">Returns the list of course types.</response>
        /// <response code="500">If there is an error retrieving data from the database.</response>
        [HttpGet("courses-types")]
        public async Task<ActionResult<IEnumerable<CoursesTypeDto>>> GetCoursesTypes()
        {
            try
            {
                var coursesTypes = await context.CoursesTypes.Where(c =>!c.IsDelete)
                    .Select(ct => new CoursesTypeDto
                    {
                        Code = ct.Code,
                        ArabicName = ct.ArabicName
                    })
                    .ToListAsync(); // Ensure that you have `using Microsoft.EntityFrameworkCore;`

                return Ok(coursesTypes);
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "خطا اثناء استرجاع تصنيفات الدورات" });
            }
        }


        [HttpGet("GetAllCoursesWithType")]
        public async Task<ActionResult<IEnumerable<CourseTypeWithCourses>>> GetAllCoursesWithType()
        {
            var courseTypes = await context.CoursesTypes.Where(c=>!c.IsDelete &&!c.IsHide)
                
                    .Include(cs => cs.CoursesDatas)
                .ToListAsync();

            var result = courseTypes.Select(ct => new CourseTypeWithCourses
            {
                TypeName = ct.ArabicName,
                Courses = ct.CoursesDatas.Select(cs => new CourseTypeWithCourses.Course
                {
                    CourseName = cs.HeaderAr,
                    CourseToken = cs.TokenNumber
                }).ToList()
            }).ToList();

            return Ok(result);
        }


    }



}

