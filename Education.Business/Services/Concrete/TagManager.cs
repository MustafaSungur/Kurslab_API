using AutoMapper;
using Education.Business.Services.Abstract;
using Education.Business.Exeptions;
using Education.Data.Repositories.Abstract;
using Education.Entity.DTOs.TagDTO;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Business.Services.Concrete
{
	public class TagManager : ITagService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public TagManager(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		// Etiket oluşturma
		public async Task<ServiceResult<TagResponseDto>> CreateTagAsync(TagRequestDto tagRequestDto)
		{
			var tag = _mapper.Map<Tag>(tagRequestDto);
			var createdTag = await _repositoryManager.TagRepository.CreateAsync(tag);

			var tagResponseDto = _mapper.Map<TagResponseDto>(createdTag);
			return ServiceResult<TagResponseDto>.SuccessResult(tagResponseDto);
		}

		// ID'ye göre etiket getirme
		public async Task<ServiceResult<TagResponseDto>> GetTagByIdAsync(int id)
		{
			var tag = await _repositoryManager.TagRepository.GetByIdAsync(id);
			if (tag == null)
			{
				return ServiceResult<TagResponseDto>.FailureResult("Etiket bulunamadı.");
			}

			var tagResponseDto = _mapper.Map<TagResponseDto>(tag);
			return ServiceResult<TagResponseDto>.SuccessResult(tagResponseDto);
		}

		// Tüm etiketleri getirme
		public async Task<ServiceResult<IEnumerable<TagResponseDto>>> GetAllTagsAsync()
		{
			var tags = await _repositoryManager.TagRepository.GetAll().ToListAsync();
			var tagResponseDtos = _mapper.Map<IEnumerable<TagResponseDto>>(tags);

			return ServiceResult<IEnumerable<TagResponseDto>>.SuccessResult(tagResponseDtos);
		}

		// Etiket güncelleme
		public async Task<ServiceResult<TagResponseDto>> UpdateTagAsync(int id, TagRequestDto tagRequestDto)
		{
			var tag = await _repositoryManager.TagRepository.GetByIdAsync(id);
			if (tag == null)
			{
				return ServiceResult<TagResponseDto>.FailureResult("Etiket bulunamadı.");
			}

			_mapper.Map(tagRequestDto, tag);
			var updatedTag = await _repositoryManager.TagRepository.UpdateAsync(tag);

			var tagResponseDto = _mapper.Map<TagResponseDto>(updatedTag);
			return ServiceResult<TagResponseDto>.SuccessResult(tagResponseDto);
		}

		// Etiket silme
		public async Task<ServiceResult<bool>> DeleteTagAsync(int id)
		{
			var success = await _repositoryManager.TagRepository.DeleteAsync(id);
			if (!success)
			{
				return ServiceResult<bool>.FailureResult("Etiket bulunamadı veya silindi.");
			}

			return ServiceResult<bool>.SuccessResult(true);
		}
	}
}
