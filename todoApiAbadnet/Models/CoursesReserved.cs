using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace todoApiAbadnet.Models;

public partial class CoursesReserved
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string? TokenNumber { get; set; } = Guid.NewGuid().ToString();
    [Required]
    public bool StutusReserved { get; set; } = false;

    [Required]
    public bool? StutusPaidup { get; set; } = false;

    [Required]
    public decimal? Paidup { get; set; }

    public decimal Payment { get; set; } = 0;

    public int PreDiscount { get; set; } = 0;

    public bool IsView { get; set; } = false;

    public int? BalanceDue { get; set; } = 0;

    public decimal? Discount { get; set; } = 0;

    //قيمة الضريبة
    public decimal? Tax { get; set; }


    public string? Nots { get; set; }

    public bool IsDelete { get; set; } = false;
    [Required]
    public string? UserCode { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // المنفذ للفاتورة
    public string? LastUpdateUserCode { get; set; }
    // تاريخ ضرب الفاتورة
    public DateTime? LastUpdateDate { get; set; }
    [Required]
    public int CoursesSchedulessId { get; set; }
    public int? ContractDatumId { get; set; }
    public bool? IsCompany { get; set; } = false;
    [Required]
    public int StudentId { get; set; }
    public int? TypePaidId { get; set; }


    public virtual ContractDatum? ContractDatum { get; set; }
    public virtual CoursesScheduless? CoursesScheduless { get; set; }
    public virtual Student? Student { get; set; }
    public virtual TypePaid? TypePaid { get; set; }


}
