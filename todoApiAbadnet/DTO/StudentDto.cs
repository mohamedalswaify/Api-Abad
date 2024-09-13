using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.DTO
{
    public class StudentDto
    {
        public int Id { get; set; }
        public string? Token { get; set; }
        public string ArabicName { get; set; }
        public string? Idnumber { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Countries { get; set; }
        public string? Nationality { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? EducationsType { get; set; }
        public string? City { get; set; }
        public string? Nots { get; set; }
        public string? Image { get; set; }
        public bool IsDelete { get; set; }
        public bool IsBlock { get; set; }
        public bool IsLocked { get; set; }
        public decimal Amount { get; set; }
        public string? UserCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastUpdateUserCode { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
