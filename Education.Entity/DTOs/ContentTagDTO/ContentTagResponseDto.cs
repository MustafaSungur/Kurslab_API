
namespace Education.Entity.DTOs.ContentTagDTO
{
	public class ContentTagResponseDto
	{
		public long ContentId { get; set; }

		public string? ContentTitle { get; set; } // İsteğe bağlı olarak içerik başlığı eklenebilir

		public int TagId { get; set; }

		public string? TagName { get; set; } // İsteğe bağlı olarak etiket ismi eklenebilir
	}
}
