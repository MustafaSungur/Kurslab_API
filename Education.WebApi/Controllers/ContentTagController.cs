using Education.Business.Services.Abstract;
using Education.Entity.DTOs.ContentTagDTO;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContentTagController : ControllerBase
	{
		private readonly IServiceManager _manager;

		public ContentTagController(IServiceManager manager)
		{
			_manager = manager;
		}

		// ContentId'ye göre Tag'leri getirme
		[HttpGet("GetTagsByContent/{contentId}")]
		public async Task<IActionResult> GetTagsByContentId(long contentId)
		{
			var result = await _manager.ContentTagService.GetTagsByContentIdAsync(contentId);
			return Ok(result.Data);
		}

		// TagId'ye göre Content'leri getirme
		[HttpGet("GetContentsByTag/{tagId}")]
		public async Task<IActionResult> GetContentsByTagId(int tagId)
		{
			var result = await _manager.ContentTagService.GetContentsByTagIdAsync(tagId);
			return Ok(result.Data);
		}
	}
}
