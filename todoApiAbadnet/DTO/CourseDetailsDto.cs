namespace todoApiAbadnet.DTO
{
    public class CourseDetailsDto
    {
        public string CourseName { get; set; }
        public string IsOnline { get; set; }
        public string FormattedTime { get; set; }
        public string CourseContent { get; set; }
        public string WhatsAppGroupLink { get; set; }
        public string DownloadFilesLink { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Recordings { get; set; }
    }

}
