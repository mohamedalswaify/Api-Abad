using System.ComponentModel.DataAnnotations;
namespace todoApiAbadnet.Models
{
    public class Room
    {


        [Key]
        public int Id { get; set; }

        public string Token { get; set; }=Guid.NewGuid().ToString();
        public string RoomNameAR { get; set; } = null!;

        [Required]
        public string? RoomNameEN { get; set; }

        public bool IsHide { get; set; } = false;

        public bool IsDelete { get; set; } = false;

        public bool IsLocked { get; set; } = false;

        [Required]
        public string? UserCode { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? LastUpdateUserCode { get; set; }

        public DateTime? LastUpdateDate { get; set; }



        public virtual ICollection<CoursesScheduless>? CoursesSchedulesses { get; set; }
    }
}
