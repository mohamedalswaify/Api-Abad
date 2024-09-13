using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace todoApiAbadnet.Models;

public partial class Student
{
    [Key]
    public int Id { get; set; }

    public string Token { get; set; } = Guid.NewGuid().ToString();

    public string ArabicName { get; set; } = null!;

    [MinLength(10, ErrorMessage = "رقم الهوية لاتقل عن 10 رقم")]
    [MaxLength(10, ErrorMessage = "رقم الهوية لايزيد عن 10 رقم")]
    public string? Idnumber { get; set; }

    [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صالح")]
    [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
    public string? Email { get; set; } = null;

    [Required(ErrorMessage = "رقم الهاتف مطلوب")]
    public string? Phone { get; set; } = null;

    public string? Gender { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? Countries { set; get; }
    public string? Nationality { set; get; }


    [Required(ErrorMessage = "كلمة المرور مطلوبة")]
    [DataType(DataType.Password)]
    [MinLength(6, ErrorMessage = "كلمة المرور لاتقل عن 6 حروف")]
    [Display(Name = "كلمة المرور الجديدة")]
    public string? Password { get; set; }

    [NotMapped]
    [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب")]
    [DataType(DataType.Password)]
    [Display(Name = "تأكيد كلمة المرور الجديدة")]
    [Compare("Password", ErrorMessage = "كلمة المرور وتأكيد كلمة المرور غير متطابقين")]
    public string? ConfirmPassword { get; set; }

    public string? EducationsType { get; set; }

    public string? City { get; set; }

    public string? Nots { get; set; } = null;

    [NotMapped]
    public IFormFile? FormFiles { set; get; }

    public string? Image { get; set; } = null;

    public bool IsDelete { get; set; } = false;

    public bool IsBlock { get; set; } = false;

    public bool IsLocked { get; set; } = false;

    public decimal Amount { set; get; } = 0;

    public string? UserCode { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;


    public string? LastUpdateUserCode { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public virtual ICollection<CoursesReserved>? CoursesReserveds { get; set; }

    public virtual ICollection<MoveStudent>? MoveStudents { set; get; }

    public virtual ICollection<ViewStudnt>? ViewStudnts { get; set; }

    public virtual ICollection<courseBasket>? CourseBaskets { get; set; }
    public virtual ICollection<TamaraPayment>? TamaraPayments { get; set; }

    public virtual ICollection<PayTabsCallbackModel>? PayTabsCallbackModels { get; set; }
    public virtual ICollection<TabbyPayment>? TabbyPayments { get; set; }
}

