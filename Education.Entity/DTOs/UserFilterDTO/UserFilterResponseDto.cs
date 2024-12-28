using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Entity.DTOs.ApplicationUserDTO;

namespace Education.Entity.DTOs.UserFilterDTO
{
	public class UserFilterResponseDto
	{
		public IEnumerable<ApplicationUserResponseDto> Users { get; set; } = [];
		public int TotalPages { get; set; }
	}

}
