using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace todoApiAbadnet.Models;

public partial class CompaniesData
{
    [Key]
    public int Code { get; set; }

    public string Token { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string ArabicName { get; set; }

    public string? EnglishName { get; set; }

    public string? ResponsibleNameAr { get; set; }

    public string? ResponsibleNameEn { get; set; }
    public string? ResponsiblePhone { get; set; }


    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? WorkFiled { get; set; }

    public string? Image { get; set; }

    [NotMapped]
    public IFormFile FormFile { get; set; }
    public bool IsDelete { get; set; }=false;

    [Required]
    public string? UserCode { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;


    public string? LastUpdateUserCode { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public virtual ICollection<ContractDatum>? ContractData { get; set; }
}
