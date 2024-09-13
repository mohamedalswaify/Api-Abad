using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoApiAbadnet.Data;
using todoApiAbadnet.DTO;
using todoApiAbadnet.Models;

namespace todoApiAbadnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanySerivesController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public CompanySerivesController(ApplicationDbContext context)
        {
            _context = context;
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
                var coursesTypes = await _context.CoursesTypes
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error retrieving data from the database" });
            }
        }


        /// <summary>
        /// Retrieves a list of all SerivesModels.
        /// </summary>
        /// <returns>An ActionResult containing a list of SerivesModel objects.</returns>
        // GET: api/serivesmodels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SerivesModel>>> GetSerivesModels()
        {
            return await _context.SerivesModels.ToListAsync();
        }



        /// <summary>
        /// Handles the creation of a new company request.
        /// </summary>
        /// <param name="companyRequest">The company request model containing the request details.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        /// <response code="200">If the request is successfully created.</response>
        /// <response code="400">If the request model is invalid.</response>
        /// <response code="500">If there is an error processing the request.</response>
        // Controller Method
        [HttpPost("create-company-request")]
        public async Task<IActionResult> CreateCompanyRequest([FromForm] CompanyRequestDto companyRequestDto, IFormFile? attachedFile)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Create the CompanyRequest entity from DTO
                var companyRequest = new CompanyRequest
                {
                    RequestNumber = Guid.NewGuid().ToString(),
                    FullName = companyRequestDto.FullName,
                    OurEmail = companyRequestDto.OurEmail,
                    Telphone = companyRequestDto.Telphone,
                    TitleJob = companyRequestDto.TitleJob,
                    OrganizationName = companyRequestDto.OrganizationName,
                    SerivesModelId = companyRequestDto.SerivesModelId,
                    CoursesTypeId = companyRequestDto.CoursesTypeId,
                    Details = companyRequestDto.Details,
                    CreatedDate = DateTime.Now
                };

                // Handle file upload if there's a file
                if (attachedFile != null)
                {
                    // Check if file is PDF
                    if (attachedFile.ContentType != "application/pdf")
                    {
                        return BadRequest(new { message = "الملف يجب أن يكون بصيغة PDF فقط." });
                    }

                    // Sanitize and shorten file name
                    var sanitizedFileName = SanitizeFileName(Path.GetFileNameWithoutExtension(attachedFile.FileName));
                    var fileExtension = Path.GetExtension(attachedFile.FileName);
                    var fileName = sanitizedFileName + fileExtension;

                    // Create a folder with the request token
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uplodesCompany", companyRequest.RequestNumber);
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var filePath = Path.Combine(folderPath, fileName);

                    // Save the file
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await attachedFile.CopyToAsync(stream);
                    }

                    companyRequest.fileName = fileName;
                }

                // Add the CompanyRequest to the database
                _context.CompanyRequests.Add(companyRequest);
                await _context.SaveChangesAsync();

                return Ok(new { message = "تم ارسال طلبك بنجاح" });
            }
            catch (Exception ex)
            {
                // Log the exception (ex)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "حدث خطأ أثناء إرسال الطلب. يرجى مراجعة البيانات." });
            }
        }

        // Helper method to sanitize file names
        private string SanitizeFileName(string fileName)
        {
            // Replace invalid characters with underscores
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitizedFileName = new string(fileName
                .Where(c => !invalidChars.Contains(c))
                .ToArray())
                .Replace(' ', '_');

            // Truncate the file name to 30 characters if it exceeds that length
            if (sanitizedFileName.Length > 30)
            {
                sanitizedFileName = sanitizedFileName.Substring(0, 30);
            }

            return sanitizedFileName;
        }


    }
}
