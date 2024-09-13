namespace todoApiAbadnet.DTO
{
    public class CourseTypeWithCourses
    {
        public string TypeName { get; set; }
        public List<Course> Courses { get; set; }

        public class Course
        {
            public string CourseName { get; set; }
            public string CourseToken { get; set; }
        }
    }
}
