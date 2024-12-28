using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.Entity.Models
{
	public class Comment:BaseEntity
	{
		public long Id { get; set; }

		[StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
		[Required(ErrorMessage = "Description is required.")]
		public required string Description { get; set; }

		[Required(ErrorMessage = "UserId is required.")]
		public required string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public ApplicationUser? User { get; set; }

		[Required(ErrorMessage = "ContentId is required.")]
		public required long ContentId { get; set; }

		[ForeignKey(nameof(ContentId))]
		public Content? Content { get; set; }

		// List of CommentLike entities, can be null if no likes exist
		public List<CommentLike>? Likes { get; set; }
	}
}
