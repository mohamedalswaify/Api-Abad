namespace todoApiAbadnet.Models
{
    public class CourseWithCost
    {
        public int CourseId { get; set; }
        public decimal CostWithTax { get; set; }
        public  decimal? DISCOUNT { get; set; }
    }
}
