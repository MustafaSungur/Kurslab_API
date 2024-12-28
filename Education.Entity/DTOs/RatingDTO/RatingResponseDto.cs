

namespace Education.Entity.DTOs.RaitingDTO
{
	public class RatingResponseDto
	{
		public long Id { get; set; }

		public DateTime CreatedDate { get; set; }

		public long ContentId { get; set; }

		public string? ContentTitle { get; set; } // İçerik başlığı gibi isteğe bağlı alan

		public string? UserId { get; set; }

		public string? UserName { get; set; } // Kullanıcı adı gibi isteğe bağlı alan

		public int RatingValue { get; set; }
	}
}
