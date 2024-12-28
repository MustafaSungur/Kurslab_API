using AutoMapper;
using Education.Business.Services.Abstract;
using Education.Business.Exeptions;
using Education.Data.Repositories.Abstract;
using Education.Entity.DTOs.CommentLikeDTO;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Education.Entity.DTOs.ContentUserDTO;

namespace Education.Business.Services.Concrete
{
	public class CommentLikeManager : ICommentLikeService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public CommentLikeManager(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		// Yorum beğenisi oluşturma
		public async Task<ServiceResult<CommentLikeResponseDto>> CreateCommentLikeAsync(CommentLikeRequestDto commentLikeRequestDto)
		{


			var checkCommentLike = await _repositoryManager.CommentLikeRepository
			.FindByCondition(cu => cu.CommentId == commentLikeRequestDto.CommentId && cu.UserId == commentLikeRequestDto.UserId)
				.FirstOrDefaultAsync();

			if (checkCommentLike == null)
			{
				var commentLike = _mapper.Map<CommentLike>(commentLikeRequestDto);

				var createdCommentLike = await _repositoryManager.CommentLikeRepository.CreateAsync(commentLike);

				var commentLikeResponseDto = _mapper.Map<CommentLikeResponseDto>(createdCommentLike);
				return ServiceResult<CommentLikeResponseDto>.SuccessResult(commentLikeResponseDto);
			}

			return ServiceResult<CommentLikeResponseDto>.FailureResult("Yorum zaten beğenilmiş");

		}

		// ID'ye göre yorum beğenisini getir
		public async Task<ServiceResult<CommentLikeResponseDto>> GetCommentLikeByIdAsync(long id)
		{
			var commentLike = await _repositoryManager.CommentLikeRepository
				.FindByCondition(cl => cl.Id == id)
				.FirstOrDefaultAsync();

			if (commentLike == null)
			{
				return ServiceResult<CommentLikeResponseDto>.FailureResult("Yorum beğenisi bulunamadı.");
			}

			var commentLikeResponseDto = _mapper.Map<CommentLikeResponseDto>(commentLike);
			return ServiceResult<CommentLikeResponseDto>.SuccessResult(commentLikeResponseDto);
		}

		// Kullanıcı ID ve Yorum ID'ye göre yorum beğenisini getir
		public async Task<ServiceResult<CommentLikeResponseDto>> GetCommentLikeByUserAndCommentAsync(string userId, long commentId)
		{
			var commentLike = await _repositoryManager.CommentLikeRepository
				.FindByCondition(cl => cl.UserId == userId && cl.CommentId == commentId)
				.FirstOrDefaultAsync();

			if (commentLike == null)
			{
				return ServiceResult<CommentLikeResponseDto>.FailureResult("Yorum beğenisi bulunamadı.");
			}

			var commentLikeResponseDto = _mapper.Map<CommentLikeResponseDto>(commentLike);
			return ServiceResult<CommentLikeResponseDto>.SuccessResult(commentLikeResponseDto);
		}

		// Yorum beğenisini sil
		public async Task<ServiceResult<bool>> DeleteCommentLikeAsync(long id)
		{
			var success = await _repositoryManager.CommentLikeRepository.DeleteAsync(id);
			if (!success)
			{
				return ServiceResult<bool>.FailureResult("Yorum beğenisi bulunamadı veya silindi.");
			}

			return ServiceResult<bool>.SuccessResult(true);
		}

		// Tüm yorum beğenilerini getir
		public async Task<ServiceResult<IEnumerable<CommentLikeResponseDto>>> GetAllCommentLikesAsync()
		{
			var commentLikes = await _repositoryManager.CommentLikeRepository.GetAll().ToListAsync();
			var commentLikeResponseDtos = _mapper.Map<IEnumerable<CommentLikeResponseDto>>(commentLikes);

			return ServiceResult<IEnumerable<CommentLikeResponseDto>>.SuccessResult(commentLikeResponseDtos);
		}

		// Kullanıcı ID'ye göre tüm yorum beğenilerini getir
		public async Task<ServiceResult<IEnumerable<CommentLikeResponseDto>>> GetCommentLikesByUserIdAsync(string userId)
		{
			var commentLikes = await _repositoryManager.CommentLikeRepository
				.FindByCondition(cl => cl.UserId == userId)
				.ToListAsync();

			if (commentLikes.Count == 0)
			{
				return ServiceResult<IEnumerable<CommentLikeResponseDto>>.FailureResult("Kullanıcıya ait yorum beğenisi bulunamadı.");
			}

			var commentLikeResponseDtos = _mapper.Map<IEnumerable<CommentLikeResponseDto>>(commentLikes);
			return ServiceResult<IEnumerable<CommentLikeResponseDto>>.SuccessResult(commentLikeResponseDtos);
		}

		// İçerik ID'ye göre tüm yorum beğenilerini getir
		public async Task<ServiceResult<IEnumerable<CommentLikeResponseDto>>> GetCommentLikesByCommentIdAsync(long commentId)
		{
			var commentLikes = await _repositoryManager.CommentLikeRepository
				.FindByCondition(cl => cl.CommentId == commentId)
				.ToListAsync();

			if (commentLikes.Count == 0)
			{
				return ServiceResult<IEnumerable<CommentLikeResponseDto>>.FailureResult("İçeriğe ait yorum beğenisi bulunamadı.");
			}

			var commentLikeResponseDtos = _mapper.Map<IEnumerable<CommentLikeResponseDto>>(commentLikes);
			return ServiceResult<IEnumerable<CommentLikeResponseDto>>.SuccessResult(commentLikeResponseDtos);
		}
	}
}
