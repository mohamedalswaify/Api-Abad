using System.ComponentModel.DataAnnotations.Schema;

namespace todoApiAbadnet.Models
{
    public class HomeScreen
    {
        public int Id { set; get; }

        public string? Title { get; set; }

        public bool IsShowTitel { get; set; }

        public string? Description { get; set; }

        public bool IsShowDes { get; set; }

        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? FormFiles { get; set; }

        public bool IsButton { get; set; }

        public bool State { get; set; }

    }
}
