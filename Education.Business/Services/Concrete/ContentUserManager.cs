using AutoMapper;
using Education.Business.Services.Abstract;
using Education.Business.Exeptions;
using Education.Data.Repositories.Abstract;
using Education.Entity.DTOs.ContentUserDTO;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Business.Services.Concrete
{
	public class ContentUserManager : IContentUserService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public ContentUserManager(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		// Content-User ilişkisi oluşturma
		public async Task<ServiceResult<ContentUserResponseDto>> CreateContentUserAsync(ContentUserRequestDto contentUserRequestDto)
		{
			var checkContentUser = await _repositoryManager.ContentUserRepository
				.FindByCondition(cu => cu.ContentId == contentUserRequestDto.ContentId && cu.UserId == contentUserRequestDto.UserId)
				.FirstOrDefaultAsync();

			if(checkContentUser == null)
			{
				var contentUser = _mapper.Map<ContentUser>(contentUserRequestDto);
				var createdContentUser = await _repositoryManager.ContentUserRepository.CreateAsync(contentUser);

				var contentUserResponseDto = _mapper.Map<ContentUserResponseDto>(createdContentUser);
				return ServiceResult<ContentUserResponseDto>.SuccessResult(contentUserResponseDto);
			}
			return ServiceResult<ContentUserResponseDto>.FailureResult("Zaten içerik izlenmiş");
		}

		// UserId'ye göre ContentUser'ları getirme
		public async Task<ServiceResult<IEnumerable<ContentUserResponseDto>>> GetContentUsersByUserIdAsync(string userId)
		{
			var contentUsers = await _repositoryManager.ContentUserRepository
				.FindByCondition(cu => cu.UserId == userId)
				.Include(cu => cu.Content)
				.ThenInclude(c=>c!.CreatedUser)
				.ToListAsync();

			var contentUserResponseDtos = _mapper.Map<IEnumerable<ContentUserResponseDto>>(contentUsers);
			return ServiceResult<IEnumerable<ContentUserResponseDto>>.SuccessResult(contentUserResponseDtos);
		}

		// ContentId'ye göre ContentUser'ları getirme
		public async Task<ServiceResult<IEnumerable<ContentUserResponseDto>>> GetContentUsersByContentIdAsync(long contentId)
		{
			var contentUsers = await _repositoryManager.ContentUserRepository
				.FindByCondition(cu => cu.ContentId == contentId)
				.Include(cu => cu.User)
				.ToListAsync();

			var contentUserResponseDtos = _mapper.Map<IEnumerable<ContentUserResponseDto>>(contentUsers);
			return ServiceResult<IEnumerable<ContentUserResponseDto>>.SuccessResult(contentUserResponseDtos);
		}

		// ContentId ve UserId'ye göre ContentUser ilişkisini getirme
		public async Task<ServiceResult<ContentUserResponseDto>> GetContentUserByContentAndUserIdAsync(long contentId, string userId)
		{
			var contentUser = await _repositoryManager.ContentUserRepository
				.FindByCondition(cu => cu.ContentId == contentId && cu.UserId == userId)
				.FirstOrDefaultAsync();

			if (contentUser == null)
			{
				return ServiceResult<ContentUserResponseDto>.FailureResult("İlişki bulunamadı.");
			}

			var contentUserResponseDto = _mapper.Map<ContentUserResponseDto>(contentUser);
			return ServiceResult<ContentUserResponseDto>.SuccessResult(contentUserResponseDto);
		}
	}
}
