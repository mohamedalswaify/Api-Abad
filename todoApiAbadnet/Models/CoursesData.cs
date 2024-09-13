using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace todoApiAbadnet.Models;

public partial class CoursesData
{
    [Key]
    public int Id { get; set; }

    public string TokenNumber { get; set; } =Guid.NewGuid().ToString();

    [Required]
    public string HeaderAr { get; set; } = null!;

    public string? HeaderEn { get; set; } 

    [Required]
    public string? SummaryAr { get; set; } 

    public string? SammaryEn { get; set; }

 
    public string? GoalsAr { get; set; } 

    public string? GoalsEn { get; set; }

    public string? TargetAr { get; set; } 

    public string? TargetEn { get; set; }

    public string? SkillsAr { get; set; } 

    public string? SkillsEn { get; set; } 

    public string DetailsAr { get; set; } = null!;

    public string? DetailsEn { get; set; }

    public string? TestAr { get; set; }

    public string? TestEn { get; set; }

    public decimal? PriceM { get; set; }



    public string? FromalutetestAr { get; set; }

    public string? FromalutetestEn { get; set; }

    public string? Nots { get; set; }

    public bool IsHide { get; set; } = false;

    public bool IsDelete { get; set; } = false;

    public decimal Price { set; get; } = 0;

    public string? Image { get; set; }

    [NotMapped]
    public IFormFile FormFiles { get; set; }

    public string? FilesData { get; set; }

    [NotMapped]
    public IFormFile FormFileData { get; set; } 

    [Required]
    public string? UserCode { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public string? LastUpdateUserCode { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    
    public int CoursesTypeId { get; set; }

    public virtual CoursesType? CoursesType { get; set; }


    public virtual ICollection<CoursesScheduless>? CoursesSchedulesss { get; set; }

    public virtual ICollection<RequiredCourse>? RequiredCourse { get; set; }

}
