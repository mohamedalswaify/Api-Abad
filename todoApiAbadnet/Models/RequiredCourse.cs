namespace todoApiAbadnet.Models
{
    public class RequiredCourse
    {
        public int Id { get; set; }

        public string? UsserName { get; set; }

        public string? UsserEmail { get; set; }

        public string? UserPhone { get; set; }

        public string? UserCity { get; set;}

        public string? Nots { get; set; }

        public bool IsAribic { get; set; } = false;

        public DateTime DateNw { get; set; }=DateTime.Now;

        public int CoursesTypeCode { get; set;}

        public virtual CoursesType? CoursesType { get; set; }

        public int CoursesDataId { get; set; }

        public virtual CoursesData? CoursesData { get; set; }



    }
}
