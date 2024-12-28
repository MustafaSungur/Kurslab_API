
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Education.Entity.Models
{
	public class Rating
	{
		
		public long Id { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public required long ContentId { get; set; }

		[ForeignKey(nameof(ContentId))]
		public Content? Content { get; set; }

		public required string UserId { get; set; }

		[ForeignKey(nameof(UserId))]
		public ApplicationUser? User { get; set; }  

		[Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
		public int RatingValue { get; set; }

    }
}
