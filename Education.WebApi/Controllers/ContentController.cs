using Education.Business.Services.Abstract;
using Education.Entity.DTOs.ContentDTO;
using Microsoft.AspNetCore.Mvc;

using Education.Business.Core.@abstract;
using Education.Entity.DTOs.ContentFilterRequestDTO;
using MediaToolkit;
using MediaToolkit.Model;
using Microsoft.AspNetCore.Authorization;
using Education.Entity.DTOs.CommentDTO;
using System.Security.Claims;
using Education.Entity.Enums;
using Microsoft.AspNetCore.Identity;
using Education.Entity.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Education.WebApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ContentController : ControllerBase
	{
		private readonly IServiceManager _manager;
		private readonly IVideoConversionService _videoConversionService;
		private readonly IFileValidatorService _fileValidatorService;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly UserManager<ApplicationUser> _userManager;

		public ContentController(IServiceManager manager, IVideoConversionService videoConversionService, IFileValidatorService fileValidatorService, IWebHostEnvironment webHostEnvironment, UserManager<ApplicationUser> userManager)
		{
			_manager = manager;
			_videoConversionService = videoConversionService;
			_fileValidatorService = fileValidatorService;
			_webHostEnvironment = webHostEnvironment;
			_userManager = userManager;
		}

		// En yüksek rating'e sahip içerikleri döndüren API endpoint'i
		[HttpGet("GetTopContents")]
		public async Task<IActionResult> GetTopContents(int pageNumber = 1, int pageSize = 10)
		{
			var result = await _manager.ContentService.GetTopContents(pageNumber, pageSize);

			if (result.Success)
			{
				return Ok(result.Data);
			}

			return BadRequest(result.ErrorMessage);
		}

		// İçerik bilgilerini ID'ye göre getiren endpoint
		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetContentById(long id)
		{
			var result = await _manager.ContentService.GetContentByIdAsync(id);

			if (result.Success)
			{
				return Ok(result.Data);
			}

			return NotFound(result.ErrorMessage); 
		}

		// Filtreleme endpoint'i
		[HttpPost("FilterContents")]
		public async Task<IActionResult> FilterContents([FromBody] ContentFilterRequestDto filterRequest)
		{
			var result = await _manager.ContentService.FilterContents(filterRequest);
			
			if (result.Success)
			{
				return Ok(result.Data); // Filtrelenmiş içerikleri döndür
			}
			return BadRequest(result.ErrorMessage); 
		}


		// User id ye göre contentleri getirir
		[Authorize(Roles = "UserAndInstructor")]
		[HttpGet("GetByUser/{id}")]
		public async Task<IActionResult> GetByUserId(string id)
		{
			var result = await _manager.ContentService.GetContentsByUserId(id);

			if (result.Success)
			{
				return Ok(result.Data);
			}

			return NotFound(result.ErrorMessage);
		}

		// İçerik Oluşturma
		[Authorize(Roles = "UserAndInstructor")]
		[HttpPost("Create")]
		public async Task<IActionResult> PostContent([FromForm] ContentRequestDto contentRequestDto, IFormFile videoFile, IFormFile? imageFile)
		{
			string? videoUrl;
			string? imageUrl = contentRequestDto.ImageUrl;
			double videoDuration = 0;

			// Video dosyasını yükleme ve dönüştürme
			if (videoFile != null && videoFile.Length > 0)
			{
				// Video uzantısın ve boyutunun kontrolü
				if (!_fileValidatorService.IsValidVideoFile(videoFile))
				{
					return BadRequest("Geçersiz video dosyası. Dosya uzantısını ve boyutunu kontrol edin.");
				}

				var videoDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "videos");
				Directory.CreateDirectory(videoDirectory);

				var originalFileName = Guid.NewGuid().ToString() + Path.GetExtension(videoFile.FileName);
				var originalFilePath = Path.Combine(videoDirectory, originalFileName);

				using (var stream = new FileStream(originalFilePath, FileMode.Create))
				{
					await videoFile.CopyToAsync(stream);
				}

				videoUrl = Path.Combine("uploads", "videos", originalFileName);

				// Video süresini almak için MediaToolkit kullanımı
				var inputFile = new MediaFile { Filename = originalFilePath };

				using (var engine = new Engine())
				{
					engine.GetMetadata(inputFile);
					videoDuration = inputFile.Metadata.Duration.TotalSeconds; // Video süresini saniye cinsinden alıyoruz
				}

				// DTO'ya video süresini ekleme
				contentRequestDto.Duration = videoDuration;
			}
			else
			{
				return BadRequest("Video dosyası gerekli. Lütfen bir video dosyası yükleyin.");
			}

			// Resim dosyasını yükleme
			if (imageFile != null && imageFile.Length > 0)
			{
				// Image uzantısın ve boyutunun kontrolü
				if (!_fileValidatorService.IsValidImageFile(imageFile))
				{
					return BadRequest("Geçersiz resim dosyası. Dosya uzantısını ve boyutunu kontrol edin.");
				}

				var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "images");
				Directory.CreateDirectory(imageDirectory);

				var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
				var imageFilePath = Path.Combine(imageDirectory, imageFileName);

				using (var stream = new FileStream(imageFilePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				imageUrl = Path.Combine("uploads", "images", imageFileName);
			}

			// Dosya URL'lerini DTO'ya ekleyip ContentManager'a gönderiyoruz
			contentRequestDto.VideoUrl = videoUrl!;
			contentRequestDto.ImageUrl = imageUrl;

			var result = await _manager.ContentService.CreateContentAsync(contentRequestDto);

			if (result.Success)
			{
				return Ok(result.Data); // Başarılıysa içerik bilgilerini döndür
			}
			else
			{
				return BadRequest(result.ErrorMessage); // Başarısızsa hata mesajını döndür
			}
		}

		// İçerik güncelleme için endpoint
		[Authorize(Roles = "UserAndInstructor")]
		[HttpPut("Update/{id}")]
		public async Task<IActionResult> UpdateContent(int id, [FromForm] ContentRequestDto contentRequestDto, IFormFile? imageFile)
		{

			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (currentUserId == null)
			{
				return Unauthorized("Kullanıcı oturum açmamış.");
			}

			// Eğer güncellenmek istenen içerik  mevcut kullanıcının değilse
			if (currentUserId != contentRequestDto.UserId)
			{
				return BadRequest("Yalnızca kendi yorumunuzu güncelleyebilirsiniz.");
			}

			string? imageUrl = contentRequestDto.ImageUrl;								

			
			// Resim dosyasını yükleme
			if (imageFile != null && imageFile.Length > 0)
			{

				// Image uzantısın ve boyutunun kontrolü
				if (!_fileValidatorService.IsValidImageFile(imageFile))
				{
					return BadRequest("Geçersiz resim dosyası. Dosya uzantısını ve boyutunu kontrol edin.");
				}

				var oldcontent = _manager.ContentService.GetContentByIdAsync(id) ?? null;
				var oldImage = oldcontent!.Result!.Data!.ImageUrl ?? null;

				if (oldcontent != null && oldImage!=null)
				{
					// Eski resim dosyasının tam yolunu al
					var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, oldImage);

					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath); // Eski resmi sil
					}
				}

			

				var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "images");
				Directory.CreateDirectory(imageDirectory);

				var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
				var imageFilePath = Path.Combine(imageDirectory, imageFileName);

				using (var stream = new FileStream(imageFilePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				imageUrl = Path.Combine("uploads", "images", imageFileName);
			}

			// Güncellenen URL'leri DTO'ya ekleyip ContentManager'a gönderiyoruz
			contentRequestDto.ImageUrl = imageUrl;

			var result = await _manager.ContentService.UpdateContentAsync(id, contentRequestDto);

			if (result.Success)
			{
				return Ok(result.Data); 
			}
			else
			{
				return BadRequest(result.ErrorMessage); 
			}
		}

		// Content analizini getiren endpoint
		[Authorize(Roles = "UserAndInstructor,Admin")]
		[HttpGet("Statistics")]
		public async Task<IActionResult> GetStatistics()
		{
			var result = await _manager.ContentService.GetContentsStatisticsAsync();

			if (result.Success)
			{
				return Ok(result.Data); 
			}

			return NotFound(result.ErrorMessage); 
		}



		// İçeriği ID'ye göre silen endpoint
		[Authorize(Roles = "UserAndInstructor,Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteContent(long id)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			var currentUser = await _userManager.FindByIdAsync(currentUserId!);
			var currentUserRole = await _userManager.GetRolesAsync(currentUser!);


			if (currentUserId == null)
			{
				return Unauthorized("Kullanıcı oturum açmamış.");
			}

			var content = await _manager.ContentService.GetContentByIdAsync(id);

			if (!content.Success)
			{
				return BadRequest("İçerik Bulunamadı");
			}

			// Eğer güncellenmek istenen içerik  mevcut kullanıcının değilse
			if (currentUserId != content!.Data!.CreatedUser!.Id && !currentUserRole.Contains(UserRole.Admin.ToString()))
			{
				return BadRequest("Yalnızca kendi yorumunuzu güncelleyebilirsiniz.");
			}

			var result = await _manager.ContentService.DeleteContentAsync(id);

			if (result.Success)
			{
				return Ok(result.Data);
			}

			return NotFound(result.ErrorMessage); 
		}

	}
}
