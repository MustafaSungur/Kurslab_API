

namespace Education.Entity.DTOs.ContentDTO
{
	public class ContentRequestDto
	{
		public required string? Title { get; set; }

		public required string? Description { get; set; }

		public  string? VideoUrl { get; set; }

		public  string? ImageUrl { get; set; }

		public required string UserId { get; set; }

        public double? Duration { get; set; }

        public required int CategoryId { get; set; }

        public required List<int> TagIds { get; set; }
    }
}
