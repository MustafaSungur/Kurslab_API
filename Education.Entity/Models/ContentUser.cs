
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.Entity.Models
{
	public class ContentUser : BaseEntity
	{
        public required string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser? User { get; set; }

		public required long ContentId { get; set; }

		[ForeignKey(nameof(ContentId))]
		public Content? Content { get; set; }
	}
}
