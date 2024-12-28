using AutoMapper;
using Education.Business.Services.Abstract;
using Education.Business.Exeptions;
using Education.Data.Repositories.Abstract;
using Education.Entity.DTOs.CommentDTO;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Education.Entity.Enums;

namespace Education.Business.Services.Concrete
{
	public class CommentManager : ICommentService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public CommentManager(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		// Yorum oluşturma
		public async Task<ServiceResult<CommentResponseDto>> CreateCommentAsync(CommentRequestDto commentRequestDto)
		{
			var comment = _mapper.Map<Comment>(commentRequestDto);
			var createdComment = await _repositoryManager.CommentRepository.CreateAsync(comment);

			var commentResponseDto = _mapper.Map<CommentResponseDto>(createdComment);
			return ServiceResult<CommentResponseDto>.SuccessResult(commentResponseDto);
		}

		// ID'ye göre yorum getirme
		public async Task<ServiceResult<CommentResponseDto>> GetCommentByIdAsync(long id)
		{
			var comment = await _repositoryManager.CommentRepository.GetByIdAsync(id);
			if (comment == null)
			{
				return ServiceResult<CommentResponseDto>.FailureResult("Yorum bulunamadı.");
			}

			var commentResponseDto = _mapper.Map<CommentResponseDto>(comment);
			return ServiceResult<CommentResponseDto>.SuccessResult(commentResponseDto);
		}

		// Tüm yorumları getirme
		public async Task<ServiceResult<IEnumerable<CommentResponseDto>>> GetAllCommentsAsync()
		{
			var comments = await _repositoryManager.CommentRepository.GetAll().ToListAsync();
			var commentResponseDtos = _mapper.Map<IEnumerable<CommentResponseDto>>(comments);

			return ServiceResult<IEnumerable<CommentResponseDto>>.SuccessResult(commentResponseDtos);
		}

		// Yorum güncelleme
		public async Task<ServiceResult<CommentResponseDto>> UpdateCommentAsync(long id, CommentRequestDto commentRequestDto)
		{
			var comment = await _repositoryManager.CommentRepository.GetByIdAsync(id);
			if (comment == null)
			{
				return ServiceResult<CommentResponseDto>.FailureResult("Yorum bulunamadı.");
			}

			comment.Description= commentRequestDto.Description;
			comment.UpdatedDate = DateTime.UtcNow;
			var updatedComment = await _repositoryManager.CommentRepository.UpdateAsync(comment);

			var commentResponseDto = _mapper.Map<CommentResponseDto>(updatedComment);
			return ServiceResult<CommentResponseDto>.SuccessResult(commentResponseDto);
		}

		// Yorum silme
		public async Task<ServiceResult<bool>> DeleteCommentAsync(long id)
		{
			var success = await _repositoryManager.CommentRepository.DeleteAsync(id);
			if (!success)
			{
				return ServiceResult<bool>.FailureResult("Yorum bulunamadı veya silindi.");
			}

			return ServiceResult<bool>.SuccessResult(true);
		}

		// Kullanıcı ID'ye göre yorumları getirme
		public async Task<ServiceResult<IEnumerable<CommentResponseDto>>> GetCommentsByUserIdAsync(string userId)
		{
			var comments = await _repositoryManager.CommentRepository
			.FindByCondition(c => c.UserId == userId && c.State != State.Deleted) 
			.ToListAsync();


			if (comments.Count == 0)
			{
				return ServiceResult<IEnumerable<CommentResponseDto>>.FailureResult("Kullanıcıya ait yorum bulunamadı.");
			}

			var commentResponseDtos = _mapper.Map<IEnumerable<CommentResponseDto>>(comments);
			return ServiceResult<IEnumerable<CommentResponseDto>>.SuccessResult(commentResponseDtos);
		}

		// İçerik ID'ye göre yorumları getirme
		public async Task<ServiceResult<IEnumerable<CommentResponseDto>>> GetCommentsByContentIdAsync(long contentId)
		{
			var comments = await _repositoryManager.CommentRepository
				.FindByCondition(c => c.ContentId == contentId && c.State != State.Deleted)
				.Include(c => c.User)          
				.Include(c => c.Likes)        
				.ToListAsync();

			if (comments.Count == 0)
			{
				return ServiceResult<IEnumerable<CommentResponseDto>>.FailureResult("İçeriğe ait yorum bulunamadı.");
			}

			var commentResponseDtos = comments.Select(_mapper.Map<CommentResponseDto>).ToList();

			return ServiceResult<IEnumerable<CommentResponseDto>>.SuccessResult(commentResponseDtos);
		}


		// Yorum ID ve Kullanıcı ID'ye göre yorumu getirme
		public async Task<ServiceResult<CommentResponseDto>> GetCommentByUserAndCommentIdAsync(long commentId, string userId)
		{
			var comment = await _repositoryManager.CommentRepository
				.FindByCondition(c => c.Id == commentId && c.UserId == userId)
				.FirstOrDefaultAsync();

			if (comment == null)
			{
				return ServiceResult<CommentResponseDto>.FailureResult("Yorum bulunamadı.");
			}

			var commentResponseDto = _mapper.Map<CommentResponseDto>(comment);
			return ServiceResult<CommentResponseDto>.SuccessResult(commentResponseDto);
		}
	}
}
