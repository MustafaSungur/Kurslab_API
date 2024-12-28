

namespace Education.Entity.DTOs.CategoryDTO
{
	public class CategoryRequestDto
	{
		public required string Name { get; set; }
        public int? ParentId { get; set; }
    }
}
