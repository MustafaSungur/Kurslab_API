using System.ComponentModel.DataAnnotations;

namespace Education.Entity.Models
{
	public class Tag:BaseEntity
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Tag name is required.")]
		[StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters.")]
		public required string Name { get; set; }

		public List<ContentTag>? ContentTags { get; set; }
	}
}
