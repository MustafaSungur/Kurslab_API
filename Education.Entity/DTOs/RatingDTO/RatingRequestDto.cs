
namespace Education.Entity.DTOs.RaitingDTO
{
	public class RatingRequestDto
	{
		public long ContentId { get; set; }

		public string? UserId { get; set; }

		public int RatingValue { get; set; } // 1 ile 5 arasında bir değer olacak
	}
}
