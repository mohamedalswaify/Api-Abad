namespace todoApiAbadnet.Models
{
    public class DiscountCodes
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public decimal Discount { get; set; } = 0;

        public int? NumberOfCourse { get; set; }

        public bool IsActive { get; set; } = true;

        public string UserCode { get; set; } = null;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? UserUpdate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
