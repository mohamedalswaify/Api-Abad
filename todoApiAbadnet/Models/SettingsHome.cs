using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.Models
{
    public class SettingsHome
    {
        public int Id { get; set; }

        public string? TitlePlane { get; set; }
        public string? ShortDescription { get; set; }
        public string? WhoMe1 { get; set; }
        public string? WhoMe2 { get; set; }
        public string? WhoMe3 { get; set; }
        public string? WhoMe4 { get; set; }
        public string? TitleVideo { get; set; }

        public string? lVideoURL { get; set; }

        [NotMapped]
        public IFormFile? formFile { get; set; }


        public string? Phone { get; set; }

        public string? WahtsAppNumber { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? LinkedinURL { get; set; }

        public string? GoogleURL { get; set; }

        public string? FacebookURL { get; set; }

        public string? YoutubeURL { get; set; }

        public string? TwitterURL { get; set; }

        public string? InstagramURL { get; set; }

        public string? SnapchatURL { get; set; }


    }
}
