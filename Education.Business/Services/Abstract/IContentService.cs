
using Education.Business.Exeptions;
using Education.Entity.DTOs.ContentDTO;
using Education.Entity.DTOs.ContentFilterDTO;
using Education.Entity.DTOs.ContentFilterRequestDTO;

namespace Education.Business.Services.Abstract
{
	public interface IContentService
	{
		Task<ServiceResult<ContentFilterResponseDto>> GetTopContents(int pageNumber, int pageSize);
		Task<ServiceResult<ContentResponseDto>> CreateContentAsync(ContentRequestDto contentRequestDto);
		Task<ServiceResult<ContentResponseDto>> UpdateContentAsync(int id, ContentRequestDto contentRequestDto);
		Task<ServiceResult<ContentResponseDto>> GetContentByIdAsync(long id);
		Task<ServiceResult<bool>> DeleteContentAsync(long id);
		Task<ServiceResult<ContentFilterResponseDto>> FilterContents(ContentFilterRequestDto filterRequest);
		Task<ServiceResult<IEnumerable<ContentResponseDto>>> GetContentsByUserId(string userId);
		Task<ServiceResult<ContentStatisticsResponseDto>> GetContentsStatisticsAsync();
	}
}
