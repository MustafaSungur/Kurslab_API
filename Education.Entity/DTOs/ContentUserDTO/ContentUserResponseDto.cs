using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Entity.DTOs.ContentDTO;

namespace Education.Entity.DTOs.ContentUserDTO
{
	public class ContentUserResponseDto
	{
		public string? UserId { get; set; } 
		public string? UserName { get; set; } 

		public long ContentId { get; set; }
		public ContentResponseDto? Content { get; set; } 
		public DateTime CreatedDate { get; set; } // İçeriğin ne zaman izlendiği (optional)
	}

}
