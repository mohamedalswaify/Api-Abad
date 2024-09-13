using Microsoft.CodeAnalysis;

namespace todoApiAbadnet.Models
{
    public class EmployeePremotion
    {
        
        public int Id { get; set; }
        public bool CoursesData { get; set; } = false;
        public bool CoursesScheduless { get; set; } = false;
        public bool CoursesStutes { get; set; } = false;
        public bool Matdetory { get; set; } = false;
        public bool ViewsNow { get; set; } = false;
        public bool RequiredCourse { get; set; } = false;
        public bool Article { get; set; } = false;
        public bool Company { get; set; } = false;
        public bool Contract { get; set; } = false;
        public bool CompanyRequest { get; set; } = false;
        public bool Studnt { get; set; } = false;
        public bool CoursesType { get; set; } = false;
        public bool TypePaid { get; set; } = false;
        public bool EmployeePage { get; set; } = false;
        public bool HomePage { get; set; } = false;
        public bool Room { get; set; } = false;
        public bool Zoom { get; set; } = false;
        public bool Complaints { get; set; } = false;
        public bool Admin { get; set; } = false;
        public int EmployeeId { get; set; }
        public virtual Employee? employee { get; set; }
    }
}
