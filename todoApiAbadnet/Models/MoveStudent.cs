using System.ComponentModel.DataAnnotations.Schema;


namespace todoApiAbadnet.Models
{
    public class MoveStudent
    {
        public int Id { get; set; }

        public int CoursesSchedulessId { get; set; }

        public int ToCourses { get; set; }

        public int EmpId { set; get; }
        
        public decimal? Amount { set; get; }

        public DateTime DateTraform { set; get; }

        public string? Nots { set; get; }

        public int StudntId { get; set; }

        [ForeignKey("StudntId")]
        public virtual Student Student { get; set; }

        public virtual CoursesScheduless CoursesScheduless { set; get; }

    }
}
