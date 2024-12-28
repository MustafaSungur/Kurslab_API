

using Education.Business.Exeptions;
using Education.Entity.DTOs.CommentLikeDTO;

namespace Education.Business.Services.Abstract
{
	public interface ICommentLikeService
	{
		Task<ServiceResult<CommentLikeResponseDto>> CreateCommentLikeAsync(CommentLikeRequestDto commentLikeRequestDto);
		Task<ServiceResult<CommentLikeResponseDto>> GetCommentLikeByIdAsync(long id);
		Task<ServiceResult<CommentLikeResponseDto>> GetCommentLikeByUserAndCommentAsync(string userId, long commentId);
		Task<ServiceResult<bool>> DeleteCommentLikeAsync(long id);
		Task<ServiceResult<IEnumerable<CommentLikeResponseDto>>> GetAllCommentLikesAsync();
		Task<ServiceResult<IEnumerable<CommentLikeResponseDto>>> GetCommentLikesByUserIdAsync(string userId);
		Task<ServiceResult<IEnumerable<CommentLikeResponseDto>>> GetCommentLikesByCommentIdAsync(long commentId);
	}
}
