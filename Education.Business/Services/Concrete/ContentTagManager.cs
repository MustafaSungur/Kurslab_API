using AutoMapper;
using Education.Business.Services.Abstract;
using Education.Business.Exeptions;
using Education.Data.Repositories.Abstract;
using Education.Entity.DTOs.ContentTagDTO;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Business.Services.Concrete
{
	public class ContentTagManager : IContentTagService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public ContentTagManager(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		// ContentId'ye göre Tag'leri getirme
		public async Task<ServiceResult<IEnumerable<ContentTagResponseDto>>> GetTagsByContentIdAsync(long contentId)
		{
			var contentTags = await _repositoryManager.ContentTagRepository
				.FindByCondition(ct => ct.ContentId == contentId)
				.Include(ct => ct.Tag)
				.ToListAsync();

			var contentTagResponseDtos = _mapper.Map<IEnumerable<ContentTagResponseDto>>(contentTags);
			return ServiceResult<IEnumerable<ContentTagResponseDto>>.SuccessResult(contentTagResponseDtos);
		}

		// TagId'ye göre Content'leri getirme
		public async Task<ServiceResult<IEnumerable<ContentTagResponseDto>>> GetContentsByTagIdAsync(int tagId)
		{
			var contentTags = await _repositoryManager.ContentTagRepository
				.FindByCondition(ct => ct.TagId == tagId)
				.Include(ct => ct.Content)
				.ToListAsync();

			var contentTagResponseDtos = _mapper.Map<IEnumerable<ContentTagResponseDto>>(contentTags);
			return ServiceResult<IEnumerable<ContentTagResponseDto>>.SuccessResult(contentTagResponseDtos);
		}
	}
}
