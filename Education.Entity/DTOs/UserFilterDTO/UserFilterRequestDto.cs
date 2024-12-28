
using Education.Entity.Enums;

namespace Education.Entity.DTOs.UserFilterDTO
{
	public class UserFilterRequestDto
	{
		public string? SearchTerm { get; set; } 
		public UserRole? Role { get; set; } 
		public int PageNumber { get; set; } = 1; 
		public int PageSize { get; set; } = 10; 
	}

}
