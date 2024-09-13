using System.ComponentModel.DataAnnotations;

namespace todoApiAbadnet.Models
{
	public class ContactUs
	{
		public int Id { get; set; } // Primary key
		public string FullName { get; set; } // Full name of the user
		[EmailAddress]
		public string Email { get; set; } // Email address
		public string MobileNumber { get; set; } // Mobile number
		public string RequestType { get; set; } // Type of request
		public string? Message { get; set; } // User's message
		public DateTime CreatedDate { get; set; } = DateTime.Now;  // Date the contact form was submitted

	}
}
