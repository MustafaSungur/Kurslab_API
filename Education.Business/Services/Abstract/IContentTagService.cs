using Education.Business.Exeptions;
using Education.Entity.DTOs.ContentTagDTO;

namespace Education.Business.Services.Abstract
{
	public interface IContentTagService
	{
		Task<ServiceResult<IEnumerable<ContentTagResponseDto>>> GetTagsByContentIdAsync(long contentId);
		Task<ServiceResult<IEnumerable<ContentTagResponseDto>>> GetContentsByTagIdAsync(int tagId);
	}
}
