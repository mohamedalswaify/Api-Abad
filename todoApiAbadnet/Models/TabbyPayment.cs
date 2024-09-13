namespace todoApiAbadnet.Models
{
    public class TabbyPayment
    {
        public int Id { get; set; }
        public string? paymentId { get; set; }
        public string TokenPayment { get; set; }
        public string checkout_url { get; set; }
        public string status { get; set; }
        public string? amountRequest { get; set; }
        public string? amountResponse { get; set; }

        public int StudentId { get; set; }
        public bool isDelete { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }

        public virtual Student? Students { get; set; }
        public virtual ICollection<TabbyPaymentCourse>? TabbyPaymentCourses { get; set; }

    }
}
