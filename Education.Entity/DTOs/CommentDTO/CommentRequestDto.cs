

namespace Education.Entity.DTOs.CommentDTO
{
	public class CommentRequestDto
	{
		public required string Description { get; set; }

		public required string UserId { get; set; }

		public long ContentId { get; set; }
	}
}
