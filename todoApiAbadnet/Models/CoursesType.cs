using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace todoApiAbadnet.Models;

public partial class CoursesType
{
    [Key]
    public int Code { get; set; }
    public string TokenNumber { get; set; } = Guid.NewGuid().ToString();
    public string ArabicName { get; set; } = null!;
    [Required]
    public string? EnglishName { get; set; }
    public bool IsHide { get; set; }=false;
    public int filt { get; set; }
    public bool IsDelete { get; set; } = false;
    public string? Image { get; set; }
    [NotMapped]
    public IFormFile FormFile { get; set; }

    public string? UserCode { get; set; }=null!;
    public DateTime CreatedDate { get; set; }=DateTime.Now;
    public string? LastUpdateUserCode { get; set; } = null!;
    public DateTime? LastUpdateDate { get; set; }= DateTime.Now;
    public virtual ICollection<CoursesData>? CoursesDatas { get; set; }
    public virtual ICollection<CoursesScheduless>? CoursesSchedulesses  { get; set; }
    public virtual ICollection<ContractDatum>? ContractData { get; set; }
    public virtual ICollection<RequiredCourse>? RequiredCourse { get; set; }
    public virtual ICollection<Article>? Article { get; set; }

    public virtual ICollection<CompanyRequest>? CompanyRequests { get; set; }
}
