using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.DTO
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage = "كلمة المرور القديمة مطلوبة")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "كلمة المرور الجديدة مطلوبة")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "كلمة المرور الجديدة لا تقل عن 6 حروف")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "كلمة المرور الجديدة وتأكيد كلمة المرور غير متطابقين")]
        public string ConfirmPassword { get; set; }
    }
}
