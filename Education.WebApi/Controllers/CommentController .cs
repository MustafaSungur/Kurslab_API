using System.Security.Claims;
using Education.Business.Services.Abstract;
using Education.Entity.DTOs.CommentDTO;
using Education.Entity.DTOs.ContentDTO;
using Education.Entity.Enums;
using Education.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CommentController : ControllerBase
	{
		private readonly IServiceManager _manager;
		private readonly UserManager<ApplicationUser> _userManager;

		public CommentController(IServiceManager manager, UserManager<ApplicationUser> userManager)
		{
			_manager = manager;
			_userManager = userManager;
		}

		// Yorum oluşturma
		[Authorize]
		[HttpPost("Create")]
		public async Task<IActionResult> CreateComment([FromBody] CommentRequestDto commentRequestDto)
		{
			var result = await _manager.CommentService.CreateCommentAsync(commentRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return BadRequest(result.ErrorMessage);
		}

		// ID'ye göre yorum getirme
		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCommentById(long id)
		{
			var result = await _manager.CommentService.GetCommentByIdAsync(id);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Tüm yorumları getirme
		[Authorize(Roles = "Admin")]
		[HttpGet("GetAll")]
		public async Task<IActionResult> GetAllComments()
		{
			var result = await _manager.CommentService.GetAllCommentsAsync();
			return Ok(result.Data);
		}

		// Kullanıcı ID'ye göre yorumları getirme
		[Authorize]
		[HttpGet("GetByUser/{userId}")]
		public async Task<IActionResult> GetCommentsByUserId(string userId)
		{
			var result = await _manager.CommentService.GetCommentsByUserIdAsync(userId);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// İçerik ID'ye göre yorumları getirme
		[Authorize]
		[HttpGet("GetByContent/{contentId}")]
		public async Task<IActionResult> GetCommentsByContentId(long contentId)
		{
			var result = await _manager.CommentService.GetCommentsByContentIdAsync(contentId);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Yorum ID ve Kullanıcı ID'ye göre yorumu getirme
		[Authorize]
		[HttpGet("GetByUserAndComment/{commentId}/{userId}")]
		public async Task<IActionResult> GetCommentByUserAndCommentId(long commentId, string userId)
		{
			
			var result = await _manager.CommentService.GetCommentByUserAndCommentIdAsync(commentId, userId);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Yorum güncelleme
		[Authorize]
		[HttpPut("Update/{id}")]
		public async Task<IActionResult> UpdateComment(long id, [FromBody] CommentRequestDto commentRequestDto)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (currentUserId == null)
			{
				return Unauthorized("Kullanıcı oturum açmamış.");
			}

			// Eğer güncellenmek istenen yorum  mevcut kullanıcının değilse
			if (currentUserId != commentRequestDto.UserId)
			{
				return BadRequest("Yalnızca kendi yorumunuzu güncelleyebilirsiniz.");
			}

			var result = await _manager.CommentService.UpdateCommentAsync(id, commentRequestDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}
			return NotFound(result.ErrorMessage);
		}

		// Yorum silme
		[Authorize]
		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> DeleteComment(long id)
		{

			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var currentUser = await _userManager.FindByIdAsync(currentUserId!);
			var currentUserRole = await _userManager.GetRolesAsync(currentUser!);

			if (currentUserId == null)
			{
				return Unauthorized("Kullanıcı oturum açmamış.");
			}

			var comment =await _manager.CommentService.GetCommentByIdAsync(id);

			if (!comment.Success)
			{
				return BadRequest("Yorum Bulunamadı.");
			}

			// Eğer silinmek istenen yorum mevcut kullanıcının değilse
			if (currentUserId != comment.Data!.User!.Id && !currentUserRole.Contains(UserRole.Admin.ToString()))
			{
				return BadRequest("Yalnızca kendi yorumunuzu silebilirsiniz.");
			}

			var result = await _manager.CommentService.DeleteCommentAsync(id);
			if (result.Success)
			{
				return Ok("Yorum başarıyla silindi.");
			}
			return NotFound(result.ErrorMessage);
		}
	}
}
