using AutoMapper;
using Education.Business.Exeptions;
using Education.Business.Services.Abstract;
using Education.Data.Repositories.Abstract;
using Education.Entity.DTOs.CategoryDTO;
using Education.Entity.DTOs.ContentDTO;
using Education.Entity.DTOs.ContentFilterDTO;
using Education.Entity.DTOs.ContentFilterRequestDTO;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Business.Services.Concrete
{
	public class ContentManager : IContentService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public ContentManager(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		// En yüksek puanlı içerikleri sayfa numarasına ve sayfa boyutuna göre getirir.
		public async Task<ServiceResult<ContentFilterResponseDto>> GetTopContents(int pageNumber, int pageSize)
		{
			try
			{
				// Toplam içerik sayısını al
				int totalContents = await _repositoryManager.ContentRepository.GetTotalContentCountAsync();

				// En yüksek puanlı içerikleri al
				var topContents = await _repositoryManager.ContentRepository.GetTopContentsAsync(pageNumber, pageSize);

				// İçerikleri DTO'ya dönüştür
				var contentDtos = topContents.Select(_mapper.Map<ContentResponseDto>).ToList();

				// Toplam sayfa sayısını hesapla
				int totalPages = (int)Math.Ceiling(totalContents / (double)pageSize);

				// Yanıt DTO'sunu oluştur
				var response = new ContentFilterResponseDto
				{
					Contents = contentDtos,
					TotalPages = totalPages,
					TotalContents = totalContents
				};

				return ServiceResult<ContentFilterResponseDto>.SuccessResult(response);
			}
			catch (Exception ex)
			{
				return ServiceResult<ContentFilterResponseDto>.FailureResult($"En yüksek puanlı içerikleri getirirken bir hata oluştu: {ex.Message}");
			}
		}

		// User id ye göre contentleri getirir
		public async Task<ServiceResult<IEnumerable<ContentResponseDto>>> GetContentsByUserId(string userId)
		{
			try
			{
				var contents = await _repositoryManager.ContentRepository.GetContentsByUserId(userId);
				var contentsDto = contents.Select(_mapper.Map<ContentResponseDto>).ToList();

				return ServiceResult<IEnumerable<ContentResponseDto>>.SuccessResult(contentsDto);
			}
			catch (Exception ex)
			{
				return ServiceResult<IEnumerable<ContentResponseDto>>.FailureResult($"En yüksek puanlı içerikleri getirirken bir hata oluştu: {ex.Message}");
			}
		}

		// ID'ye göre içerik detaylarını getirir.
		public async Task<ServiceResult<ContentResponseDto>> GetContentByIdAsync(long id)
		{
			try
			{
				var content = await _repositoryManager.ContentRepository.GetByIdAsync(id);
				if (content == null)
				{
					return ServiceResult<ContentResponseDto>.FailureResult("İçerik bulunamadı.");
				}

				var contentResponseDto = _mapper.Map<ContentResponseDto>(content);
				return ServiceResult<ContentResponseDto>.SuccessResult(contentResponseDto);
			}
			catch (Exception ex)
			{
				return ServiceResult<ContentResponseDto>.FailureResult($"İçerik getirilirken bir hata oluştu: {ex.Message}");
			}
		}

		// Yeni bir içerik oluşturur ve veritabanına kaydeder.
		public async Task<ServiceResult<ContentResponseDto>> CreateContentAsync(ContentRequestDto contentRequestDto)
		{
			try
			{
				if (contentRequestDto.TagIds.Count < 1 || contentRequestDto.TagIds.Count > 3)
				{
					return ServiceResult<ContentResponseDto>.FailureResult("Etiket sayısı en az 1 en fazla 3 olabilir");
				}

				var content = _mapper.Map<Content>(contentRequestDto);
				content.ImageUrl = contentRequestDto.ImageUrl ?? string.Empty;
				
				var createdContent = await _repositoryManager.ContentRepository.CreateAsync(content);

				createdContent.Rating = 0;
				createdContent.RatingCount = 0;
				
				// Etiketleri ContentTag olarak ekle
				foreach (var tagID in contentRequestDto.TagIds)
				{
					var contentTag = new ContentTag { ContentId = createdContent.Id, TagId = tagID };
					var createdContentTag = await _repositoryManager.ContentTagRepository.CreateAsync(contentTag);
				}

				var contentResponseDto = _mapper.Map<ContentResponseDto>(createdContent);

				return ServiceResult<ContentResponseDto>.SuccessResult(contentResponseDto);
			}
			catch (Exception ex)
			{
				return ServiceResult<ContentResponseDto>.FailureResult($"İçerik oluşturulurken bir hata oluştu: {ex.Message}");
			}
		}

		// Var olan bir içeriği günceller.
		public async Task<ServiceResult<ContentResponseDto>> UpdateContentAsync(int id, ContentRequestDto contentRequestDto)
		{
			try
			{
				var existingContent = await _repositoryManager.ContentRepository.GetByIdAsync(id);
				if (existingContent == null)
				{
					return ServiceResult<ContentResponseDto>.FailureResult("İçerik bulunamadı.");
				}

				existingContent.Title = contentRequestDto.Title ?? existingContent.Title;
				existingContent.Description = contentRequestDto.Description ?? existingContent.Description;
				existingContent.ImageUrl = contentRequestDto.ImageUrl ?? existingContent.ImageUrl;
				existingContent.UpdatedDate = DateTime.UtcNow;
				existingContent.CategoryId = contentRequestDto.CategoryId;

				var exitstingContentTags = await _repositoryManager.ContentTagRepository.GetContentTagsByContentIdAsync(existingContent.Id);

				// Eski content tag ları sil
				var existingContentTags = await _repositoryManager.ContentTagRepository.GetContentTagsByContentIdAsync(existingContent.Id);

				var createdContentTag = await _repositoryManager.ContentTagRepository.DeleteRange(existingContentTags.ToArray());



				// Etiketleri ContentTag olarak ekle
				foreach (var tagID in contentRequestDto.TagIds)
				{
					var contentTag = new ContentTag { ContentId = existingContent.Id, TagId = tagID };
					var newTags = await _repositoryManager.ContentTagRepository.CreateAsync(contentTag);
				}

				await _repositoryManager.ContentRepository.UpdateAsync(existingContent);

				var contentResponseDto = _mapper.Map<ContentResponseDto>(existingContent);
				return ServiceResult<ContentResponseDto>.SuccessResult(contentResponseDto);
			}
			catch (Exception ex)
			{
				return ServiceResult<ContentResponseDto>.FailureResult($"İçerik güncellenirken bir hata oluştu: {ex.Message}");
			}
		}

		// ID'ye göre içeriği siler.
		public async Task<ServiceResult<bool>> DeleteContentAsync(long id)
		{
			try
			{
				var content = await _repositoryManager.ContentRepository.GetByIdAsync(id);

				if (content == null)
				{
					return ServiceResult<bool>.FailureResult("İçerik bulunamadı.");
				}

				await _repositoryManager.ContentRepository.DeleteAsync(id);

				var comments = await _repositoryManager.CommentRepository
				.FindByCondition(c => c.ContentId == id)
				.ToListAsync();

                foreach (var comment in comments)
                {
					await _repositoryManager.CommentRepository.DeleteAsync(comment.Id);
                }

                return ServiceResult<bool>.SuccessResult(true);
			}
			catch (Exception ex)
			{
				return ServiceResult<bool>.FailureResult($"İçerik silinirken bir hata oluştu: {ex.Message}");
			}
		}
		
		public async Task<ServiceResult<ContentFilterResponseDto>> FilterContents(ContentFilterRequestDto filterRequest)
		{
			// Tüm içerikleri veritabanından al, yorum sayısını da içerecek şekilde
			var query = _repositoryManager.ContentRepository.GetAll()
				.Select(c => new
				{
					Content = c,
					CommentCount = c.Comments != null ? c.Comments.Count : 0 // Yorum sayısını hesapla
				});

			// Kategori filtresi
			if (filterRequest.CategoryId.HasValue)
			{
				// Alt kategorileri bul
				var categoryIds = await GetCategoryAndChildrenIdsAsync(filterRequest.CategoryId.Value);

				// İçerikleri filtrele (hem seçilen kategori hem de alt kategorilere göre)
				query = query.Where(c => categoryIds.Contains(c.Content.CategoryId));
			}

			// Etiket filtresi
			if (filterRequest.TagIds != null && filterRequest.TagIds.Count != 0)
			{
				query = query.Where(c => c.Content.ContentTags!.Any(ct => filterRequest.TagIds.Contains(ct.TagId)));
			}

			// Arama filtresi (Title'a göre arama yap)
			if (!string.IsNullOrEmpty(filterRequest.SearchTerm))
			{
				query = query.Where(c => c.Content.Title.ToLower().Contains(filterRequest.SearchTerm.ToLower()));
			}

			// Toplam içerik sayısını al
			int totalContents = await query.CountAsync();

			// Sayfalama işlemi
			var contents = await query
				.Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
				.Take(filterRequest.PageSize)
				.ToListAsync();

			// İçerikleri DTO'ya dönüştür
			var contentDtos = contents.Select(c =>
			{
				var contentDto = _mapper.Map<ContentResponseDto>(c.Content);
				contentDto.CommentCount = c.CommentCount; // Yorum sayısını DTO'ya ekle
				return contentDto;
			}).ToList();

			// Toplam sayfa sayısını hesapla
			int totalPages = (int)Math.Ceiling(totalContents / (double)filterRequest.PageSize);

			// Yanıt DTO'sunu oluştur
			var response = new ContentFilterResponseDto
			{
				Contents = contentDtos,
				TotalPages = totalPages,
				TotalContents = totalContents
			};

			return ServiceResult<ContentFilterResponseDto>.SuccessResult(response);
		}

		private async Task<List<int>> GetCategoryAndChildrenIdsAsync(int categoryId)
		{
			// Kategori ve alt kategorilerinin ID'lerini bulmak için bir liste oluştur
			var categoryIds = new List<int> { categoryId };

			// Alt kategorileri al
			var childCategories = await _repositoryManager.CategoryRepository.GetAll()
				.Where(c => c.ParentId == categoryId)
				.ToListAsync();

			// Alt kategorileri ekle
			foreach (var childCategory in childCategories)
			{
				categoryIds.AddRange(await GetCategoryAndChildrenIdsAsync(childCategory.Id));
			}

			return categoryIds;
		}


		public async Task<ServiceResult<ContentStatisticsResponseDto>> GetContentsStatisticsAsync()
		{
			try
			{
				// Toplam eğitim ve izlenme sayısını al
				var (totalCourses, totalViews) = await _repositoryManager.ContentRepository.GetTotalCoursesAndViewsAsync();

				// Kategorilere göre eğitim ve izlenme oranlarını al
				var categoryStatistics = await _repositoryManager.CategoryRepository.GetCategoryStatisticsAsync();

				var categoryStatisticsResponseDto = categoryStatistics.Select(_mapper.Map<CategoryStatisticsResponseDto>).ToList();

				var contentStatisticsResponseDto = new ContentStatisticsResponseDto
				{
					TotalCourses = totalCourses,
					TotalViews = totalViews,
					CategoryStatistics = categoryStatisticsResponseDto
				};

				return ServiceResult<ContentStatisticsResponseDto>.SuccessResult(contentStatisticsResponseDto);
			}
			catch (Exception ex)
			{
				return ServiceResult<ContentStatisticsResponseDto>.FailureResult("İstatistikleri getirirken bir sorun oluştu: " + ex.Message);
			}
		}

	}
}
