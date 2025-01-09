using Education.Business.Core.@abstract;
using Education.Business.Services.Abstract;
using Education.Entity.DTOs.ApplicationUserDTO;
using Education.Entity.DTOs.UserFilterDTO;
using Education.Entity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Education.WebApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationUserController : ControllerBase
	{
		private readonly IServiceManager _manager;
		private IWebHostEnvironment _webHostEnvironment;
		private readonly IFileValidatorService _fileValidatorService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly IConfiguration _configuration;

		public ApplicationUserController(IServiceManager manager, IWebHostEnvironment webHostEnvironment,IFileValidatorService fileValidatorService, UserManager<ApplicationUser> userManager,ITokenService tokenService, IConfiguration configuration)
		{
			_manager = manager;
			_webHostEnvironment = webHostEnvironment;
			_fileValidatorService = fileValidatorService;
			_userManager = userManager;
			_tokenService = tokenService;
			_configuration = configuration;
		}

		// Yeni kullanıcı oluşturma
		[HttpPost("Create")]
		public async Task<IActionResult> CreateUser([FromForm] ApplicationUserRequestDto userRequestDto, IFormFile? imageFile)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			string? imageUrl=userRequestDto.Image;

			// Resim dosyasını yükleme
			if (imageFile != null && imageFile.Length > 0)
			{

				// Image uzantısın ve boyutunun kontrolü
				if (!_fileValidatorService.IsValidImageFile(imageFile))
				{
					return BadRequest("Geçersiz resim dosyası. Dosya uzantısını ve boyutunu kontrol edin.");
				}

				var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profileImages");
				Directory.CreateDirectory(imageDirectory);

				var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
				var imageFilePath = Path.Combine(imageDirectory, imageFileName);

				using (var stream = new FileStream(imageFilePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				imageUrl = Path.Combine("uploads", "profileImages", imageFileName);
			}
			
			userRequestDto.Image=imageUrl;

			var result = await _manager.ApplicationUserService.CreateUserAsync(userRequestDto);

			if (result.Success)
			{
				return Ok(result.Data);
			}

			return BadRequest(result.ErrorMessage); 
		}

		// Kullanıcıyı ID ile getirme
		[Authorize]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserById(string id)
		{
			var result = await _manager.ApplicationUserService.GetUserByIdAsync(id);
			if (result.Success)
			{
				return Ok(result.Data); 
			}

			return NotFound(result.ErrorMessage); 
		}

		// Kullanıcı güncelleme
		[Authorize]
		[HttpPut("Update/{id}")]
		[Consumes("multipart/form-data")]
		public async Task<IActionResult> UpdateUser( string id, [FromForm] ApplicationUserRequestDto updatedUserDto, IFormFile? imageFile)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (currentUserId == null)
			{
				return Unauthorized("Kullanıcı oturum açmamış.");
			}

			var currentUser = await _userManager.FindByIdAsync(currentUserId!);


			// Eğer güncellenmek istenen kullanıcı mevcut kullanıcı değilse VEYA mevcut kullanıcı admin değilse
			if (currentUserId != id && !await _userManager.IsInRoleAsync(currentUser!, "Admin"))
			{
				return BadRequest("Yalnızca kendi profilinizi güncelleyebilir veya admin yetkisine sahip olmalısınız.");
			}

			string? imageUrl = updatedUserDto.Image;

			// Eğer yeni bir resim dosyası varsa ve eski bir resim URL'si varsa, eski resmi sil
			if (imageFile != null && imageFile.Length > 0)
			{
					var user = await _manager.ApplicationUserService.GetUserByIdAsync(id);
					
					if(user == null)
					{
						return BadRequest("Kullanıcı Bulunamadı");
					}

					// Eski resim dosyasının tam yolunu al
					var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, user.Data!.Image!);

					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath); // Eski resmi sil
					}
				

				// Yeni resim dosyasını yükleme
				if (!_fileValidatorService.IsValidImageFile(imageFile))
				{
					return BadRequest("Geçersiz resim dosyası. Dosya uzantısını ve boyutunu kontrol edin.");
				}

				var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "profileImages");
				Directory.CreateDirectory(imageDirectory);

				var imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
				var imageFilePath = Path.Combine(imageDirectory, imageFileName);

				using (var stream = new FileStream(imageFilePath, FileMode.Create))
				{
					await imageFile.CopyToAsync(stream);
				}

				imageUrl = Path.Combine("uploads", "profileImages", imageFileName);
			}

			updatedUserDto.Image = imageUrl;

			var result = await _manager.ApplicationUserService.UpdateUserAsync(id, updatedUserDto);
			if (result.Success)
			{
				return Ok(result.Data);
			}

			return BadRequest(result.ErrorMessage);
		}


		[Authorize]
		[HttpPost("ActiveRol/{id}")]
		public async Task<IActionResult> ActivateInstructorRole(string id)
		{
			var applicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			var applicationUser = await _userManager.FindByIdAsync(applicationUserId!);

			if (applicationUser == null)
			{
				return BadRequest("Kullanıcı Bulunamadı.");
			}

			// Eğitmen rolünü aktif etme işlemi
			var resultActive = await _manager.ApplicationUserService.ActivateInstructorRole(id);

			if (!resultActive.Success)
			{
				return BadRequest(resultActive.ErrorMessage);
			}

			// Token ve token süresi oluşturma işlemi
			var updatedUser = await _userManager.FindByIdAsync(applicationUserId!);
			var token = await _tokenService.GenerateToken(updatedUser!);
			var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]));

			return Ok(new { token, expiration });
		}


		// Kullanıcı analizlerini getiren endpoint
		[Authorize(Roles = "Admin")]
		[HttpGet("Analysis")]
		public async Task<IActionResult> GetAllUsersAnalysis()
		{
			var result = await _manager.ApplicationUserService.GetAllUsersAnalysisAsync();

			if (result.Success)
			{
				return Ok(result.Data);
			}

			return BadRequest(result.ErrorMessage);
		}

		// Kullanıcıları filtreleyen endpoint
		[Authorize(Roles = "Admin")]
		[HttpPost("Filter")]
		public async Task<IActionResult> FilterUsers([FromBody] UserFilterRequestDto filterRequest)
		{
			var result = await _manager.ApplicationUserService.FilterUsersAsync(filterRequest);

			if (result.Success)
			{
				return Ok(result.Data);
			}

			return BadRequest(result.ErrorMessage);
		}

		// Kullanıcı silme
		[Authorize]
		[HttpDelete("Delete/{id}")]
		public async Task<IActionResult> DeleteUser(string id)
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (currentUserId == null)
			{
				return Unauthorized("Kullanıcı oturum açmamış.");
			}

			var currentUser = await _userManager.FindByIdAsync(currentUserId!);

			// Eğer Silinmek istenen kullanıcı mevcut kullanıcı değilse VEYA mevcut kullanıcı admin değilse
			if (currentUserId != id && !await _userManager.IsInRoleAsync(currentUser!, "Admin"))
			{
				return BadRequest("Yalnızca kendi hesabınızı silebilirsiniz veya admin yetkisine sahip olmalısınız.");
			}

			var result = await _manager.ApplicationUserService.DeleteUserAsync(id);

			if (result.Success)
			{
				return Ok(result.SuccessMessage);
			}

			return BadRequest(result.ErrorMessage); 
		}
	}
}
