using System.ComponentModel.DataAnnotations.Schema;

namespace Education.Entity.Models
{
	public class ContentTag:BaseEntity
	{
		public long ContentId { get; set; }

		[ForeignKey(nameof(ContentId))]
		public Content? Content { get; set; }

		public int TagId { get; set; }

		[ForeignKey(nameof(TagId))]
		public Tag? Tag { get; set; }
	}
}
