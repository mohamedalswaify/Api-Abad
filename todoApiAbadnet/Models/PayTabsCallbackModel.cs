namespace todoApiAbadnet.Models
{
    public class PayTabsCallbackModel
    {
        public int Id { get; set; }
        public string? AcquirerMessage { get; set; }
        public string? AcquirerRRN { get; set; }
        public string? CartId { get; set; }
        public string? CustomerEmail { get; set; }
        public string? RespCode { get; set; }
        public string? RespMessage { get; set; }
        public string? RespStatus { get; set; }
        public string? Token { get; set; }
        public string? TranRef { get; set; }
        public decimal Amount { get; set; } = 0;
        public decimal? Discount { get; set; } = 0;
        public int? StudentId { get; set; }
        public string? Nots { get; set; }
        public DateTime? CretedDate { get; set; } = DateTime.Now;
        public DateTime? DateAfterPay { get; set; } = DateTime.Now;
        public virtual Student? Students { get; set; }

        public virtual ICollection<paytabsPaymentCourse>? PaytabsPaymentCourses { get; set; }

    }
}
