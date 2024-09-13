using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.Models
{
    public class TypePaid
    {

        [Key]
        public int Id { get; set; }

        public string ArabicName { get; set; } = null!;

        [Required]
        public string? EnglishName { get; set; }= null!;

        public bool IsHide { get; set; } = false;

        public bool IsDelete { get; set; } = false;

        public bool IsLocked { get; set; }=false;


        [Required]
        public string? UserCode { get; set; }=null!;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string? LastUpdateUserCode { get; set; } = null!;

        public DateTime? LastUpdateDate { get; set; }=DateTime.Now;



        public virtual ICollection<CoursesReserved>? CoursesReserveds { get; set; }



    }
}
