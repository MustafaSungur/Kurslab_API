
using Education.Business.Exeptions;
using Education.Entity.DTOs.CategoryDTO;

namespace Education.Business.Services.Abstract
{
	public interface ICategoryService
	{
		Task<ServiceResult<CategoryResponseDto>> CreateCategoryAsync(CategoryRequestDto categoryRequestDto);
		Task<ServiceResult<CategoryResponseDto>> GetCategoryByIdAsync(int id);
		Task<ServiceResult<IEnumerable<CategoryResponseDto>>> GetAllCategoriesAsync();
		Task<ServiceResult<CategoryResponseDto>> UpdateCategoryAsync(int id, CategoryRequestDto categoryRequestDto);
		Task<ServiceResult<bool>> DeleteCategoryAsync(int id);
	}
}
