using AutoMapper;
using Education.Business.Services.Abstract;
using Education.Business.Exeptions;
using Education.Data.Repositories.Abstract;
using Education.Entity.DTOs.CategoryDTO;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Business.Services.Concrete
{
	public class CategoryManager : ICategoryService
	{
		private readonly IRepositoryManager _repositoryManager;
		private readonly IMapper _mapper;

		public CategoryManager(IRepositoryManager repositoryManager, IMapper mapper)
		{
			_repositoryManager = repositoryManager;
			_mapper = mapper;
		}

		// Kategori oluşturma
		public async Task<ServiceResult<CategoryResponseDto>> CreateCategoryAsync(CategoryRequestDto categoryRequestDto)
		{
			try
			{
				var category = _mapper.Map<Category>(categoryRequestDto);
				var createdCategory = await _repositoryManager.CategoryRepository.CreateAsync(category);

				var categoryResponseDto = _mapper.Map<CategoryResponseDto>(createdCategory);
				return ServiceResult<CategoryResponseDto>.SuccessResult(categoryResponseDto);
			}
			catch (Exception ex)
			{
				return ServiceResult<CategoryResponseDto>.FailureResult($"Kategori oluşturulurken bir hata oluştu: {ex.Message}");
			}
		}

		// ID'ye göre kategori getir
		public async Task<ServiceResult<CategoryResponseDto>> GetCategoryByIdAsync(int id)
		{
			var categories = await _repositoryManager.CategoryRepository.GetAll().ToListAsync();

			var categoryDtos = _mapper.Map<List<CategoryResponseDto>>(categories);

			// Belirtilen kategori ve alt kategorileri için ağaç yapısını oluştur
			var categoryTree = BuildCategoryTreeForId(categoryDtos, id);

			if (categoryTree == null)
			{
				return ServiceResult<CategoryResponseDto>.FailureResult("Kategori bulunamadı.");
			}

			return ServiceResult<CategoryResponseDto>.SuccessResult(categoryTree);
		}

		private CategoryResponseDto? BuildCategoryTreeForId(List<CategoryResponseDto> categories, int categoryId)
		{
			// Tüm kategorileri hızlı erişim için bir sözlüğe koy
			var lookup = categories.ToDictionary(c => c.Id);

			// İstenen kategoriyi bul
			if (!lookup.TryGetValue(categoryId, out var rootCategory))
			{
				return null; // Kategori bulunamadı
			}

			// Alt kategorileri ilişkilendir
			foreach (var category in categories)
			{
				if (category.ParentId == rootCategory.Id)
				{
					if (rootCategory.Children == null)
					{
						rootCategory.Children = new List<CategoryResponseDto>();
					}
					rootCategory.Children.Add(category);
				}
			}

			// Belirtilen kategorinin alt kategorilerinin çocuklarını da ekle
			foreach (var child in rootCategory.Children ?? new List<CategoryResponseDto>())
			{
				child.Children = BuildCategoryTreeForId(categories, child.Id)?.Children;
			}

			return rootCategory;
		}


		// Tüm kategorileri getirir
		public async Task<ServiceResult<IEnumerable<CategoryResponseDto>>> GetAllCategoriesAsync()
		{
			// Tüm kategorileri repository'den al
			var categories = await _repositoryManager.CategoryRepository.GetAll().ToListAsync();

			// AutoMapper ile DTO'ya dönüştür
			var categoryDtos = _mapper.Map<List<CategoryResponseDto>>(categories);

			// Ağaç yapısını oluştur
			var categoryTree = BuildCategoryTree(categoryDtos);

			return ServiceResult<IEnumerable<CategoryResponseDto>>.SuccessResult(categoryTree);
		}

		// Subcategorileri ağaç şeklinde oluşturuyor - GetAll ve GetById de classlarında kullanılıyor
		private static List<CategoryResponseDto> BuildCategoryTree(List<CategoryResponseDto> categories)
		{
			var lookup = categories.ToDictionary(c => c.Id);
			var roots = new List<CategoryResponseDto>();

			foreach (var category in categories)
			{
				if (category.ParentCategory == null) // Ana kategoriler
				{
					roots.Add(category);
				}
				else if (lookup.TryGetValue(category.ParentCategory.Id, out var parent)) // Alt kategoriler
				{
					parent.Children ??= [];

					parent.Children.Add(category);
				}
			}

			return roots;
		}



		// Kategori güncelle
		public async Task<ServiceResult<CategoryResponseDto>> UpdateCategoryAsync(int id, CategoryRequestDto categoryRequestDto)
		{
			var existingCategory = await _repositoryManager.CategoryRepository.GetByIdAsync(id);
			if (existingCategory == null)
			{
				return ServiceResult<CategoryResponseDto>.FailureResult("Kategori bulunamadı.");
			}

			existingCategory.Name = categoryRequestDto.Name;
			await _repositoryManager.CategoryRepository.UpdateAsync(existingCategory);

			var categoryResponseDto = _mapper.Map<CategoryResponseDto>(existingCategory);
			return ServiceResult<CategoryResponseDto>.SuccessResult(categoryResponseDto);
		}

		// Kategori sil
		public async Task<ServiceResult<bool>> DeleteCategoryAsync(int id)
		{
			var category = await _repositoryManager.CategoryRepository.GetByIdAsync(id);
			if (category == null)
			{
				return ServiceResult<bool>.FailureResult("Kategori bulunamadı.");
			}

			await _repositoryManager.CategoryRepository.DeleteAsync(id);
			return ServiceResult<bool>.SuccessResult(true);
		}
	}
}
