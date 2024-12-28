

using Education.Entity.DTOs.ApplicationUserDTO;
using Education.Entity.DTOs.CommentLikeDTO;

namespace Education.Entity.DTOs.CommentDTO
{
	public class CommentResponseDto
	{
		public long Id { get; set; }

		public required string Description { get; set; }

		public ApplicationUserResponseDto? User { get; set; }

		public string? UserName { get; set; }

		public long ContentId { get; set; }

		public string? ContentTitle { get; set; }
		
		public List<CommentLikeResponseDto>? Likes { get; set; }

        public DateTime CreatedDate { get; set; }
    }

}
