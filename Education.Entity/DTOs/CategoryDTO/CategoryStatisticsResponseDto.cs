

namespace Education.Entity.DTOs.CategoryDTO
{
	public class CategoryStatisticsResponseDto
	{
		public required string CategoryName { get; set; }
		public int CourseCount { get; set; }
		public long TotalViews { get; set; }
	}
}

