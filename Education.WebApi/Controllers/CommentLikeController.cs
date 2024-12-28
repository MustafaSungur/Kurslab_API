using Education.Business.Services.Abstract;
using Education.Entity.DTOs.CommentLikeDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CommentLikeController : ControllerBase
	{
		private readonly IServiceManager _manager;

		public CommentLikeController(IServiceManager manager)
		{
			_manager = manager;
		}

		// Yorum beğenisi oluşturma
		[Authorize]
		[HttpPost("Create")]
		public async Task<IActionResult> CreateCommentLike([FromBody] CommentLikeRequestDto commentLikeRequestDto)
		{
			var result = await _manager.CommentLikeService.CreateCommentLikeAsync(commentLikeRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return BadRequest(result.ErrorMessage);
		}

		// ID'ye göre yorum beğenisini getir
		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCommentLikeById(long id)
		{
			var result = await _manager.CommentLikeService.GetCommentLikeByIdAsync(id);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Kullanıcı ID ve Yorum ID'ye göre yorum beğenisini getir
		[Authorize]
		[HttpGet("GetByUserAndComment/{userId}/{commentId}")]
		public async Task<IActionResult> GetCommentLikeByUserAndComment(string userId, long commentId)
		{
			var result = await _manager.CommentLikeService.GetCommentLikeByUserAndCommentAsync(userId, commentId);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Yorum beğenisini sil
		[Authorize]
		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> DeleteCommentLike(long id)
		{
			var result = await _manager.CommentLikeService.DeleteCommentLikeAsync(id);
			if (result.Success)
			{
				return Ok("Yorum beğenisi başarıyla silindi.");
			}
			return NotFound(result.ErrorMessage);
		}

		// Tüm yorum beğenilerini getir
		[Authorize]
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAllCommentLikes()
		{
			var result = await _manager.CommentLikeService.GetAllCommentLikesAsync();
			return Ok(result.Data);
		}

		// Kullanıcı ID'ye göre tüm yorum beğenilerini getir
		[Authorize]
		[HttpGet("GetByUser/{userId}")]
		public async Task<IActionResult> GetCommentLikesByUserId(string userId)
		{
			var result = await _manager.CommentLikeService.GetCommentLikesByUserIdAsync(userId);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Yorum ID'ye göre tüm yorum beğenilerini getir
		[Authorize]
		[HttpGet("GetByComment/{commentId}")]
		public async Task<IActionResult> GetCommentLikesByCommentId(long commentId)
		{
			var result = await _manager.CommentLikeService.GetCommentLikesByCommentIdAsync(commentId);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}
	}
}
