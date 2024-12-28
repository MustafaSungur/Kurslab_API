using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Education.Entity.Models
{
	public class Category : BaseEntity
	{
		public int Id { get; set; }

		[Required(ErrorMessage = "Category name is required.")]
		[StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
		public required string Name { get; set; }

		public int? ParentId { get; set; }

		[ForeignKey(nameof(ParentId))]
		public Category? ParentCategory { get; set; }

		public List<Content>? Contents { get; set; }
	}
}