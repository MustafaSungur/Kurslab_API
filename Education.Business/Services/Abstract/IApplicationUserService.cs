
using Education.Business.Exeptions;
using Education.Entity.DTOs.ApplicationUserDTO;
using Education.Entity.DTOs.UserAnalysisDTO;
using Education.Entity.DTOs.UserFilterDTO;
using Microsoft.AspNetCore.Identity;

namespace Education.Business.Services.Abstract
{
	public interface IApplicationUserService
	{
		  Task<ServiceResult<ApplicationUserResponseDto>> CreateUserAsync(ApplicationUserRequestDto applicationUserRequestDto);
		  Task<ServiceResult<ApplicationUserResponseDto>> GetUserByIdAsync(string id);
		  Task<ServiceResult<ApplicationUserResponseDto>> UpdateUserAsync(string id, ApplicationUserRequestDto applicationUserRequestDto);
		  Task<ServiceResult<string>> DeleteUserAsync(string id);
		Task<ServiceResult<string>> ActivateInstructorRole(string id);
		Task<ServiceResult<UserAnalysisResponseDto>> GetAllUsersAnalysisAsync();
		Task<ServiceResult<UserFilterResponseDto>> FilterUsersAsync(UserFilterRequestDto filterRequest);
	}
}
