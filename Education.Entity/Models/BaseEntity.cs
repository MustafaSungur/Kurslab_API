using Education.Entity.Enums;

namespace Education.Entity.Models
{
	public class BaseEntity
	{
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		public DateTime? UpdatedDate { get; set; }

		public State State { get; set; } = State.Active;
	}
}
