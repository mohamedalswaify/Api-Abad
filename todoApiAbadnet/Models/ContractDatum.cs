using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace todoApiAbadnet.Models;

public partial class ContractDatum
{
    [Key]
    public int Id { get; set; }

    public string Token { get; set; } =Guid.NewGuid().ToString();

    public DateTime ContractDate { get; set; }= DateTime.Now;
    [Required]
    public int StudentCount { get; set; }

    public string? Tiltle { get; set; }

    public string? Subject { get; set; }

    public decimal ContractPrise { get; set; }

    public decimal ContractPaid { get; set; }

    public decimal ContractRest { get; set; }

    public int BranchCode { get; set; } = 1;

    public int StatusCode { get; set; } = 1;

    public string? Nots { get; set; }


    public bool IsDeletae { get; set; }= false;

    [Required]
    public string? UserCode { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public string? LastUpdateUserCode { get; set; }

    public DateTime? LastUpdateDate { get; set; }
 
    public virtual ContractStatus? ContractStatus { get; set; }

    public int CompaniesDataId { get; set; }
    public virtual CompaniesData? CompaniesData { get; set; }

    public virtual ICollection<CoursesReserved>? CoursesReserveds { get; set; }  
}
