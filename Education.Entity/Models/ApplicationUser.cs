
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Education.Entity.Enums;
using Microsoft.AspNetCore.Identity;

namespace Education.Entity.Models
{
	public class ApplicationUser : IdentityUser
	{
		[StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
		[Required(ErrorMessage = "First name is required.")]
		public required string FirstName { get; set; }

		[StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
		[Required(ErrorMessage = "Last name is required.")]
		public required string LastName { get; set; }

		public Genre Gender { get; set; } = Genre.NotSpecified;

		public UserRole Role { get; set; } = UserRole.User;

		[Required(ErrorMessage = "Birth date is required.")]
		[DataType(DataType.Date)]
		public required DateTime BirthDate { get; set; }
					
		public DateTime CreatedDate { get; set; } =  DateTime.UtcNow;

		public DateTime? UpdatedDate { get; set; }

		// State field, defaulting to Active
		public State State { get; set; } = State.Active;

		public string Image { get; set; } = string.Empty;

		[NotMapped]
		[DataType(DataType.Password)]
		[Required(ErrorMessage = "Password is required.")]
		[StringLength(100, ErrorMessage = "Password must be between 6 and 100 characters.", MinimumLength = 6)]
		public string? Password { get; set; }

		[NotMapped]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
		public string? ConfirmPassword { get; set; }

		public List<Rating>? Ratings { get; set; }

		public List<ContentUser>? ViewedContents{ get; set; }

		public List<Comment>? Comments { get; set; }

		[NotMapped]
		public List<Tag> Tags { get; set; } = [];



	}
}
