using Education.Business.Services.Abstract;
using Education.Entity.DTOs.ContentUserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContentUserController : ControllerBase
	{
		private readonly IServiceManager _manager;

		public ContentUserController(IServiceManager manager)
		{
			_manager = manager;
		}

		// Content-User ilişkisi oluşturma
		[Authorize]
		[HttpPost("Create")]
		public async Task<IActionResult> CreateContentUser([FromBody] ContentUserRequestDto contentUserRequestDto)
		{
			var result = await _manager.ContentUserService.CreateContentUserAsync(contentUserRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return BadRequest(result.ErrorMessage);
		}

		// UserId'ye göre ContentUser'ları getirme
		[Authorize]
		[HttpGet("GetByUser/{userId}")]
		public async Task<IActionResult> GetContentUsersByUserId(string userId)
		{
			var result = await _manager.ContentUserService.GetContentUsersByUserIdAsync(userId);
			return Ok(result.Data);
		}

		// ContentId'ye göre ContentUser'ları getirme
		[Authorize]
		[HttpGet("GetByContent/{contentId}")]
		public async Task<IActionResult> GetContentUsersByContentId(long contentId)
		{
			var result = await _manager.ContentUserService.GetContentUsersByContentIdAsync(contentId);
			return Ok(result.Data);
		}

		// ContentId ve UserId'ye göre ilişkiyi getirme
		[Authorize]
		[HttpGet("GetByContentAndUser/{contentId}/{userId}")]
		public async Task<IActionResult> GetContentUserByContentAndUserId(long contentId, string userId)
		{
			var result = await _manager.ContentUserService.GetContentUserByContentAndUserIdAsync(contentId, userId);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}
	}
}
