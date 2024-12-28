using Education.Business.Services.Abstract;
using Education.Entity.DTOs.CategoryDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CategoryController : ControllerBase
	{
		private readonly IServiceManager _manager;

		public CategoryController(IServiceManager manager)
		{
			_manager = manager;
		}

		// Kategori oluşturma
		[Authorize(Roles = "Admin")]
		[HttpPost("Create")]
		public async Task<IActionResult> CreateCategory([FromBody] CategoryRequestDto categoryRequestDto)
		{
			var result = await _manager.CategoryService.CreateCategoryAsync(categoryRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return BadRequest(result.ErrorMessage);
		}

		// ID'ye göre kategori getir
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategoryById(int id)
		{
			var result = await _manager.CategoryService.GetCategoryByIdAsync(id);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Tüm kategorileri getir
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAllCategories()
		{
			var result = await _manager.CategoryService.GetAllCategoriesAsync();
			return Ok(result.Data);
		}

		// Kategori güncelle
		[Authorize(Roles = "Admin")]
		[HttpPut("Update/{id}")]
		public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDto categoryRequestDto)
		{
			var result = await _manager.CategoryService.UpdateCategoryAsync(id, categoryRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Kategori sil
		[Authorize(Roles = "Admin")]
		[HttpDelete("Delete/{id}")]
		
		public async Task<IActionResult> DeleteCategory(int id)
		{
			var result = await _manager.CategoryService.DeleteCategoryAsync(id);
			if (result.Success)
			{
				return Ok("Kategori başarıyla silindi.");
			}
			return NotFound(result.ErrorMessage);
		}
	}
}
