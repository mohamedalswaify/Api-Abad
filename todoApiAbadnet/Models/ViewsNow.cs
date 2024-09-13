using System.ComponentModel.DataAnnotations.Schema;
namespace todoApiAbadnet.Models
{
    public class ViewsNow
    {
        public int Id { set; get; }

        public int NumberOfCourses { set; get; }

        public string? Descraption { set; get; }

        public string? Head { set; get; }

        public DateTime? DateOpen { set; get; }

        public DateTime? DateClosed { set; get;}

        public bool IsOpen { set; get; } = false;

        public int PreDiscount { set; get; }

        public bool IsDelete { get; set; } = false;

        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? FormFiles { get; set; }

        public string? UserCode { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? LastUpdateUserCode { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public virtual ICollection<ViewStudnt> ViewStudnts { get; set; }
    }
}
