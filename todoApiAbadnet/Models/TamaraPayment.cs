namespace todoApiAbadnet.Models
{
    public class TamaraPayment
    {
        public int Id { get; set; }
        public string order_id { get; set; }
        public string checkout_id { get; set; }
        public string checkout_url { get; set; }
        public string status { get; set; }
        public decimal TotalAmountwithTaxAferDiscount { get; set; }
        public decimal discountAmount { get; set; } = 0;
        public int StudentId { get; set; }
        public bool isDelete { get; set; } = false;
        public DateTime Created { get; set; } = DateTime.Now;
        public virtual Student? Students { get; set; }

        public virtual ICollection<TamaraPaymentCourse>? TamaraPaymentCourses { get; set; }
    }
}
