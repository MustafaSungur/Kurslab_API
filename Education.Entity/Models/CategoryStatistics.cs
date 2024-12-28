using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Entity.Models
{
	public class CategoryStatistics
	{
		public required string CategoryName { get; set; }
		public int CourseCount { get; set; }
		public long TotalViews { get; set; }
	}
}
