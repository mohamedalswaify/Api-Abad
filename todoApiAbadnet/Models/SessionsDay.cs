using System.ComponentModel.DataAnnotations.Schema;

namespace todoApiAbadnet.Models
{
    public class SessionsDay
    {

        public int Id { get; set; }

        public string NameOfDay { get; set; }

        
        public int SessionsWeekId { get; set; }

        public virtual SessionsWeek? SessionsWeek { get; set; }

        public virtual ICollection<SessionsRecord> SessionsRecords { get; set; }

        
    }

}
