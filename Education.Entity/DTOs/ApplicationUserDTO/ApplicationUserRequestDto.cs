
using Education.Entity.Enums;

namespace Education.Entity.DTOs.ApplicationUserDTO
{
	public class ApplicationUserRequestDto
	{
		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public Genre? Gender { get; set; } = Genre.NotSpecified;

		public DateTime? BirthDate { get; set; }

		public string? Password { get; set; }

		public string? ConfirmPassword { get; set; }

		public string? Email { get; set; }

        public string? Image { get; set; }

    }
}
