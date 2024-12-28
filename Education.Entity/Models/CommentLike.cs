using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Education.Entity.Enums;

namespace Education.Entity.Models
{
	public class CommentLike
	{
		[Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "UserId is required.")]
		public required string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public ApplicationUser? User { get; set; }

		[Required(ErrorMessage = "CommentId is required.")]
		public required long CommentId { get; set; }

		[ForeignKey(nameof(CommentId))]
		public Comment? Comment { get; set; }

		public State State { get; set; } = State.Active;
	}
}
