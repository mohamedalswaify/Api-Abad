  using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace todoApiAbadnet.Models
{
    public class CompanyRequest
    {
        public int Id { get; set; }

        public string? RequestNumber { get; set; }

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


        public string? Details { get; set; }=null;


        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [NotMapped]
        public IFormFile AttachedFile { get; set; }


        public string? fileName { get; set; } = null;



        public virtual SerivesModel? SerivesModels { get; set; }

        public virtual CoursesType? CoursesTypes { get; set; }



    }
}
