namespace todoApiAbadnet.DTO
{
    public class CourseDto
    {
        public string Token { get; set; }
        public string? CourseName { get; set; }
        public DateOnly StartDate { get; set; }
        public int IsOnlineId { get; set; }
        public string IsOnline { get; set; }
        public string Hadaf { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string summaryAr {  get; set; }
        public string summary {  get; set; }
        public string GoalsAr {  get; set; }
        public string TargetAr {  get; set; }
        public string DetailsAr {  get; set; }
        public string TestAr { get; set; }
        public int NumberOfweeks { get; set; }
        public int NumberOfHours { get; set; }
        public string TrainerLanguage { get; set; }
        public string FormattedTimeStart { get; set; } // New property for formatted time
        public string FormattedTimeEnd { get; set; } // New property for formatted time
        public List<CourseDto> OpenCourses { get; set; } // Property for open courses
    }
}
