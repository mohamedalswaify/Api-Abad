namespace todoApiAbadnet.Models
{
    public class SerivesModel
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<CompanyRequest>? CompanyRequests { get; set; }


    }
}
