using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.Models;

public partial class CoursesIsonline
{
    [Key]
    public int Id { get; set; }

    [Required] 
    public string? ArabicName { get; set; } = null!; 

    public string EnglishName { get; set; } = null!;

    public bool IsHide { get; set; } = false;

    public bool IsDelete { get; set; } = false;

    [Required] 
    public string? UserCode { get; set; } = null!;

    public DateTime CreatedDate { get; set; } = DateTime.Now;


    public string? LastUpdateUserCode { get; set; }=null!;

    public DateTime? LastUpdateDate { get; set; }=DateTime.Now;

    public virtual ICollection<CoursesScheduless>? CoursesSchedulesses { get; set; }
}
