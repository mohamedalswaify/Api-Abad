using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.Models
{

    public class CampsModel
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public string? Education { get; set; }


        public string? PhoneNumber { get; set; }

        public string? TypeCourse { get; set; }

        public bool IsPayment { get; set; } = false;


        public DateTime CreatedCode { get; set; } = DateTime.Now;
    }
}
