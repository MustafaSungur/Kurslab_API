using Education.Business.Exeptions;
using Education.Entity.DTOs.CommentDTO;

namespace Education.Business.Services.Abstract
{
	public interface ICommentService
	{
		Task<ServiceResult<CommentResponseDto>> CreateCommentAsync(CommentRequestDto commentRequestDto);
		Task<ServiceResult<CommentResponseDto>> GetCommentByIdAsync(long id);
		Task<ServiceResult<IEnumerable<CommentResponseDto>>> GetAllCommentsAsync();
		Task<ServiceResult<CommentResponseDto>> UpdateCommentAsync(long id, CommentRequestDto commentRequestDto);
		Task<ServiceResult<bool>> DeleteCommentAsync(long id);
		Task<ServiceResult<IEnumerable<CommentResponseDto>>> GetCommentsByUserIdAsync(string userId);
		Task<ServiceResult<IEnumerable<CommentResponseDto>>> GetCommentsByContentIdAsync(long contentId);
		Task<ServiceResult<CommentResponseDto>> GetCommentByUserAndCommentIdAsync(long commentId, string userId);
	}
}
