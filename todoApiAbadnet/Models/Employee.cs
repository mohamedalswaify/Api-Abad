using System.Collections.ObjectModel;

namespace todoApiAbadnet.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public string Token { get; set; } = Guid.NewGuid().ToString();  
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public bool IsDelete { get; set; } = false;
        public virtual Collection<EmployeePremotion>? EmployeePremotion { get; set; }



    }
}
