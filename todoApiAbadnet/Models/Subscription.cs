using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.Models
{
    public class Subscription
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
