
using Microsoft.AspNetCore.Http;

namespace Education.Business.Core.@abstract
{
	public interface IFileValidatorService
	{
		bool IsValidVideoFile(IFormFile videoFile);
		bool IsValidImageFile(IFormFile imageFile);
	}

}
