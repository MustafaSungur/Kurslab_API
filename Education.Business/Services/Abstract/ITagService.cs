using Education.Business.Exeptions;
using Education.Entity.DTOs.TagDTO;

namespace Education.Business.Services.Abstract
{
	public interface ITagService
	{
		Task<ServiceResult<TagResponseDto>> CreateTagAsync(TagRequestDto tagRequestDto);
		Task<ServiceResult<TagResponseDto>> GetTagByIdAsync(int id);
		Task<ServiceResult<IEnumerable<TagResponseDto>>> GetAllTagsAsync();
		Task<ServiceResult<TagResponseDto>> UpdateTagAsync(int id, TagRequestDto tagRequestDto);
		Task<ServiceResult<bool>> DeleteTagAsync(int id);
	}
}
