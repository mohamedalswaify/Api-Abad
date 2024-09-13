namespace todoApiAbadnet.Models
{
    public class SessionsWeek
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<SessionsDay>? sessionsDays { get; set; }
    }
}
