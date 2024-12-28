namespace Education.Entity.DTOs.CategoryDTO
{
	public class CategoryResponseDto
	{
		public int Id { get; set; }

		public required string Name { get; set; }

        public  int ParentId { get; set; }

        public CategoryResponseDto? ParentCategory { get; set; }

		public List<CategoryResponseDto>? Children { get; set; }
	}
}

