using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace todoApiAbadnet.Models;

public partial class Branch
{
    [Key]
    public int Id { get; set; }

    public string? ArabicName { get; set; } = null!;

    public string? EnglishName { get; set; } = null!;

    public string? Address { get; set; } = null!;

    public string? Phone { get; set; } = null!;

    public string? Phone2 { get; set; } = null!;

    public string? TwitterUrl { get; set; } = null!;

    public string? FacabookUrl { get; set; } = null!;

    public string? InstagramUrl { get; set; } = null!;

    public string? Location { get; set; } = null!;

    public string? WhatsApp { get; set; } = null!;

    public string? WebsiteUrl { get; set; } = null!;

    public bool IsHide { get; set; } = false;

    public bool IsDelete { get; set; }=false;

    public int? UserCode { get; set; }=1;

    public DateTime CreatedDate { get; set; }= DateTime.Now;

    public int? LastUpdateUserCode { get; set; } = 1;

    public DateTime? LastUpdateDate { get; set; }=DateTime.Now;

    public virtual ICollection<CoursesScheduless>? CoursesSchedulesss { get; set; }
}
