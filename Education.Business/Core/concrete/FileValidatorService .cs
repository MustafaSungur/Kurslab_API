using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Education.Business.Core.@abstract;

public class FileValidatorService:IFileValidatorService
{
	private readonly long _maxVideoSize;
	private readonly long _maxImageSize;
	private readonly string[] _allowedVideoExtensions;
	private readonly string[] _allowedImageExtensions;


	public FileValidatorService(IConfiguration configuration)
	{
		_maxVideoSize = configuration.GetValue<long>("FileSettings:MaxVideoSize");
		_maxImageSize = configuration.GetValue<long>("FileSettings:MaxImageSize");
		_allowedVideoExtensions = configuration.GetSection("FileSettings:AllowedVideoExtensions").Get<string[]>()!;
		_allowedImageExtensions = configuration.GetSection("FileSettings:AllowedImageExtensions").Get<string[]>()!;
	}

	public bool IsValidVideoFile(IFormFile videoFile)
	{
		var extension = Path.GetExtension(videoFile.FileName).ToLower();
		return _allowedVideoExtensions.Contains(extension) && videoFile.Length <= _maxVideoSize;
	}

	public bool IsValidImageFile(IFormFile imageFile)
	{
		var extension = Path.GetExtension(imageFile.FileName).ToLower();
		return _allowedImageExtensions.Contains(extension) && imageFile.Length <= _maxImageSize;
	}
}
