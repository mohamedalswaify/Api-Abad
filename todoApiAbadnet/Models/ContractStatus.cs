using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace todoApiAbadnet.Models;

public partial class ContractStatus
{
    [Key]
    public int Code { get; set; }

    [Required]
    public string ArabicName { get; set; } = null!;

    public string? EnglishName { get; set; }

    public bool IsDelete { get; set; }=false;

    public int UserCode { get; set; }

    public DateTime CreatedDate { get; set; }=DateTime.Now;

    public int? LastUpdateUserCode { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public virtual ICollection<ContractDatum>? ContractData { get; set; }
}
