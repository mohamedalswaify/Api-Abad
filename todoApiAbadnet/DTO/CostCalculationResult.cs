using todoApiAbadnet.Models;

namespace todoApiAbadnet.DTO
{
    public class CostCalculationResult
    {
        public List<CourseWithCost> CoursesWithCostList { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
