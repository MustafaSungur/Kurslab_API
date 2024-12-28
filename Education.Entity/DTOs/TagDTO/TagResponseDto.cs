
using Education.Entity.DTOs.ContentTagDTO;

namespace Education.Entity.DTOs.TagDTO
{
	public class TagResponseDto
	{
		public int Id { get; set; }

		public required string Name { get; set; }

		public List<ContentTagResponseDto>? ContentTags { get; set; } 
	}
}
