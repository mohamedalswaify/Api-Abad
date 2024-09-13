namespace todoApiAbadnet.Models
{
    public class Comment
    {
        public int Id { get; set; }


        public string? StudentName { get; set; }


        public string? Studentjob { get; set; }


        public string? comment { get; set; }

        public int? evaluation { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
