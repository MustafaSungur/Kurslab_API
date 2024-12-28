using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Entity.DTOs.UserAnalysisDTO
{
	public class UserAnalysisResponseDto
	{
		public int ActiveUsers { get; set; }
		public int DeletedUsers { get; set; }
		public int RegularUsers { get; set; } 
		public int UserAndInstructors { get; set; } 
	}
}
