
using Education.Entity.DTOs.CategoryDTO;

namespace Education.Entity.DTOs.ContentDTO
{
	public class ContentStatisticsResponseDto
	{
		public int TotalCourses { get; set; }
		public long TotalViews { get; set; }
		public List<CategoryStatisticsResponseDto>? CategoryStatistics { get; set; }
	}
}
