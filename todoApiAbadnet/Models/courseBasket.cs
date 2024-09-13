namespace todoApiAbadnet.Models
{
    public class courseBasket
    {
        public int Id { get; set; }

        public string TokenNumber { get; set; } = Guid.NewGuid().ToString();


        public int CoursesSchedulessId { get; set; }

        public int StudentId { get; set; }

        

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public virtual CoursesScheduless CoursesScheduless { get; set; }

        public virtual Student Students { get; set; }
    }
}
