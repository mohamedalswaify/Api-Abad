using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace todoApiAbadnet.Models
{
    public class SessionsRecord
    {
        public int Id { get; set; }

        [NotMapped]
        public List<DateTime> listofData { get; set; }


        public int CoursesSchedulessId { get; set; }

        public string? MeatingId { get; set; }

        public string? UserEmail { get; set; }

        public string? MeatingUrl { get; set; }

        public DateTime? Date { get; set; }

        public bool IsReplcesing { get; set; }=false;

        public string? Title { get; set; }

        public int SessionsDayId { get; set; }

        public virtual SessionsDay? sessionsDay { get; set; }

        public virtual CoursesScheduless? CoursesScheduless { get; set; }

        public string? UserCode { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? LastUpdateUserCode { get; set; }

        public DateTime? LastUpdateDate { get; set; }






    }
}
