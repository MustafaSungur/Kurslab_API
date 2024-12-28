using Education.Business.Services.Abstract;
using Education.Entity.DTOs.RaitingDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RatingController : ControllerBase
	{
		private readonly IServiceManager _manager;

		public RatingController(IServiceManager manager)
		{
			_manager = manager;
		}

		// Rating oluşturma işlemi
		[Authorize]
		[HttpPost("Create")]
		public async Task<IActionResult> CreateRating([FromBody] RatingRequestDto ratingRequestDto)
		{
			var result = await _manager.RatingService.CreateRatingAsync(ratingRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return BadRequest(result.ErrorMessage);
		}
	}
}
