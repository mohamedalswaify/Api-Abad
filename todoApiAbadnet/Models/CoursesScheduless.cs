
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;


namespace todoApiAbadnet.Models;

public partial class CoursesScheduless
{
    [Key]       
    public int Id { get; set; }

    public string TokenNumber { get; set; }

    public DateOnly? StartDate { get; set; }
    
    [NotMapped]

    public DateOnly? CalculatedEndDate
    {
        get
        {
            if (StartDate.HasValue)
            {
                return StartDate.Value.AddDays(3);
            }
            else
            {
                return null;
            }
        }
    }
    public DateOnly? EndDate { get; set; }

    public TimeOnly? StartTime { get; set; }
        
    public TimeOnly? EndTime { get; set; }
    
    public int NumberOfHourse { get; set; }

    public DateOnly? CloseDay { get; set; }

    public int NumberOfWeek { get; set; }

    public decimal Cost { get; set; }


    public bool IsHadaf { get; set; }

    public bool IsFares { get; set; } = false;

    public bool IsRamdan { get; set; }

    public string? LinkWhatsApp { get; set; }

    public string? TimeCoures{ get; set; }

    public bool IsHide { get; set; }=false;

    public bool IsDelete { get; set; } = false;


    public string? UserCode { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public string? LastUpdateUserCode { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public int CoursesIsonlineId { get; set; }

    public int CoursesDataId { get; set; }

    public int TrainerId { get; set; }

    public int CoursesTypeCode { get; set; }

    public int BranchId { get; set; } = 1;

    public int CoursesStatusId { get; set; }

    public int RoomId { get; set; }

    public virtual Room? Room { get; set; }

    public virtual Trainer? Trainer { get; set; }

    public virtual CoursesData? CoursesData { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual CoursesType? CoursesType{ get; set; }

    public virtual CoursesIsonline? CoursesIsonline { get; set; }  
    
    public virtual CoursesStatus? CoursesStatus { get; set; }

    public virtual ICollection<CoursesReserved>? CoursesReserveds { get; set; }

    public virtual ICollection<SessionsRecord>? SessionsRecords { get; set; }

    public virtual ICollection<ViewStudnt>? ViewStudnts { get; set; }

    public virtual ICollection<courseBasket>? CourseBaskets { get; set; }
    public virtual ICollection<TamaraPayment>? TamaraPayments { get; set; }
    public virtual ICollection<TamaraPaymentCourse>? TamaraPaymentCourses { get; set; }
    public virtual ICollection<paytabsPaymentCourse>? PaytabsPaymentCourses { get; set; }
    public virtual ICollection<TabbyPaymentCourse>? TabbyPaymentsCourses { get; set; }









}
