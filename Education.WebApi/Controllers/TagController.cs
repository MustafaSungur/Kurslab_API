using Education.Business.Services.Abstract;
using Education.Entity.DTOs.TagDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TagController : ControllerBase
	{
		private readonly IServiceManager _manager;

		public TagController(IServiceManager manager)
		{
			_manager = manager;
		}

		// Etiket oluşturma
		[Authorize(Roles = "Admin")]
		[HttpPost("Create")]
		public async Task<IActionResult> CreateTag([FromBody] TagRequestDto tagRequestDto)
		{
			var result = await _manager.TagService.CreateTagAsync(tagRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return BadRequest(result.ErrorMessage);
		}

		// ID'ye göre etiket getirme
		[HttpGet("{id}")]
		public async Task<IActionResult> GetTagById(int id)
		{
			var result = await _manager.TagService.GetTagByIdAsync(id);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Tüm etiketleri getirme
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAllTags()
		{
			var result = await _manager.TagService.GetAllTagsAsync();
			return Ok(result.Data);
		}

		// Etiket güncelleme
		[Authorize(Roles = "Admin")]
		[HttpPut("Update/{id}")]
		public async Task<IActionResult> UpdateTag(int id, [FromBody] TagRequestDto tagRequestDto)
		{
			var result = await _manager.TagService.UpdateTagAsync(id, tagRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Etiket silme
		[Authorize(Roles = "Admin")]
		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> DeleteTag(int id)
		{
			var result = await _manager.TagService.DeleteTagAsync(id);
			if (result.Success)
			{
				return Ok("Etiket başarıyla silindi.");
			}
			return NotFound(result.ErrorMessage);
		}
	}
}
