using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.DTO
{
    public class CompanyRequestDto
    {
        public string? TokenNumber { get; set; }

        [Required(ErrorMessage = "الاسم كامل مطلوب")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
        public string OurEmail { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        public string Telphone { get; set; }

        [Required(ErrorMessage = "المسمى الوظيفي مطلوب")]
        public string TitleJob { get; set; }

        [Required(ErrorMessage = "اسم المنظمة مطلوب")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "الخدمة المطلوبة مطلوبة")]
        public int SerivesModelId { get; set; }

        [Required(ErrorMessage = "اسم الدورة مطلوب")]
        public string CoursesTypeId { get; set; }

        public string? Details { get; set; }

        // Note: CreatedDate, AttachedFile, and fileName are not included in the DTO
    }

}
