using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education.Business.Core.@abstract
{
	public interface IVideoConversionService
	{

		Task<bool> ConvertToH264(string inputFilePath, string outputFilePath);
	}
}
