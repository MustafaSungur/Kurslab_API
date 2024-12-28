using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Entity.DTOs.ContentFilterRequestDTO
{
	public class ContentFilterRequestDto
	{
		public int? CategoryId { get; set; }
		public List<int>? TagIds { get; set; }
		public string? SearchTerm { get; set; } 

		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 6;
	}


}
