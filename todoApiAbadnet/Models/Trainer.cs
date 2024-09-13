using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace todoApiAbadnet.Models;
public partial class Trainer
{
    [Key]
    public int Id { get; set; }

    public string Token { get; set; } = Guid.NewGuid().ToString();

    public int? IdNumber { get; set; }

    public string? NameAr { get; set; } = null!;

    public string? NameEn { get; set; } = null!;

    public string? Mobile { get; set; } = null!;

    public double? Salary { get; set; } = 0;

    public string? EmailPerson { get; set; } = null!;

    public string? PasswordPerson { get; set; } = null!;

    public string? EmailZoom { get; set; } = null!;

    public string? PasswordZoom { get; set; } = null!;

    public bool IsHide { get; set; } = false;

    public bool IsDelete { get; set; } = false;

    public bool IsLocked { get; set; } = false;

    [NotMapped]
    public IFormFile FormFile { get; set; }
    public string? Image { get; set; }

    public string? UserCode { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public string? LastUpdateUserCode { get; set; } = string.Empty;

    public DateTime? LastUpdateDate { get; set; } = DateTime.Now;
}
