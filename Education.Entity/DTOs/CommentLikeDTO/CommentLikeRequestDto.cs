

namespace Education.Entity.DTOs.CommentLikeDTO
{
	public class CommentLikeRequestDto
	{
		public required string UserId { get; set; }

		public required long CommentId { get; set; }
	}
}
