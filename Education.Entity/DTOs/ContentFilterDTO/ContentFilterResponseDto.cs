using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Education.Entity.DTOs.ContentDTO;

namespace Education.Entity.DTOs.ContentFilterDTO
{
	public class ContentFilterResponseDto
	{
		public IEnumerable<ContentResponseDto> Contents { get; set; } = [];
		public int TotalPages { get; set; }
		public int TotalContents { get; set; }
	}
}
