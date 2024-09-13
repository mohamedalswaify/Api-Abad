namespace todoApiAbadnet.Models
{
    public class TamaraPaymentCourse
    {
        public int Id { get; set; }
        public int TamaraPaymentId { get; set; }
        public int CoursesSchedulessId { get; set; }
        public decimal CostWithTax { get; set; } = 0;
        public decimal? DISCOUNT { get; set; } = 0;
        public string? Nots { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual TamaraPayment? TamaraPayment { get; set; }
        public virtual CoursesScheduless? CoursesScheduless { get; set; }

    }
}
