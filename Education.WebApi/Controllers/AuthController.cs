using Education.Business.Core.@abstract;
using Education.Entity.DTOs.LoginDTO;
using Education.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Education.WebApi.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly IConfiguration _configuration;

		public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService, IConfiguration configuration)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			_configuration = configuration;
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
		{
			try
			{
				var applicationUser = await _userManager.FindByEmailAsync(loginRequest.Email);

				if (applicationUser != null && await _userManager.CheckPasswordAsync(applicationUser, loginRequest.Password))
				{
					var token = await _tokenService.GenerateToken(applicationUser);
					return Ok(new
					{
						token,
						expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]))
					});
				}
				return Unauthorized();
			}
			catch (Exception ex)
			{
				// Log the exception here for debugging purposes (e.g., log to a file or a logging service)
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
			}
		}


		[HttpPost("ForgetPassword")]
		public async Task<ActionResult<string>> ForgetPassword(string email)
		{
			try
			{
				var applicationUser = await _userManager.FindByEmailAsync(email);
				if (applicationUser == null)
				{
					return NotFound("User not found.");
				}

				if (applicationUser.UserName == "Admin")
				{
					throw new Exception("Access Denied");
				}

				var token = await _userManager.GeneratePasswordResetTokenAsync(applicationUser);
				//EmailManager.SendEmail(applicationUser.Email!, "Şifre Sıfırlama", token);
				return Ok(token);
			}
			catch (Exception ex)
			{
				// Log the exception details here for debugging
				return StatusCode(StatusCodes.Status500InternalServerError, "Error sending email.");
			}
		}

		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword(string email, string token, string newPassword)
		{
			try
			{
				var applicationUser = await _userManager.FindByEmailAsync(email);
				if (applicationUser == null)
				{
					return NotFound("User not found");
				}

				if (applicationUser.UserName == "Admin")
				{
					throw new Exception("Access Denied");
				}

				var result = await _userManager.ResetPasswordAsync(applicationUser, token, newPassword);
				if (result.Succeeded)
				{
					return Ok();
				}

				return BadRequest(result.Errors);
			}
			catch (Exception ex)
			{
				// Log the exception details here for debugging
				return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while resetting the password.");
			}
		}
	}
}
