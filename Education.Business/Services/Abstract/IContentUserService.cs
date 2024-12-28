using Education.Business.Exeptions;
using Education.Entity.DTOs.ContentUserDTO;

namespace Education.Business.Services.Abstract
{
	public interface IContentUserService
	{
		Task<ServiceResult<ContentUserResponseDto>> CreateContentUserAsync(ContentUserRequestDto contentUserRequestDto);
		Task<ServiceResult<IEnumerable<ContentUserResponseDto>>> GetContentUsersByUserIdAsync(string userId);
		Task<ServiceResult<IEnumerable<ContentUserResponseDto>>> GetContentUsersByContentIdAsync(long contentId);
		Task<ServiceResult<ContentUserResponseDto>> GetContentUserByContentAndUserIdAsync(long contentId, string userId);
	}
}
