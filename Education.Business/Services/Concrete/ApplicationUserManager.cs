using AutoMapper;
using Education.Business.Services.Abstract;
using Education.Business.Exeptions;
using Education.Entity.DTOs.ApplicationUserDTO;
using Education.Entity.Models;
using Microsoft.AspNetCore.Identity;
using Education.Entity.Enums;
using Microsoft.Extensions.Configuration;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Education.Entity.DTOs.UserAnalysisDTO;
using Microsoft.EntityFrameworkCore;
using Education.Entity.DTOs.UserFilterDTO;
using Microsoft.IdentityModel.Tokens;
using Education.Data.Repositories.Abstract;

namespace Education.Business.Services.Concrete
{
	public class ApplicationUserManager : IApplicationUserService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
		private readonly IRepositoryManager _repositoryManager;
		public ApplicationUserManager(UserManager<ApplicationUser> userManager, IMapper mapper,IRepositoryManager repositoryManager)
		{
			
			_userManager = userManager;
			_mapper = mapper;
			_repositoryManager = repositoryManager;
		}

		// Yeni bir kullanıcı oluşturma
		public async Task<ServiceResult<ApplicationUserResponseDto>> CreateUserAsync(ApplicationUserRequestDto userRequestDto)
		{
			var user = _mapper.Map<ApplicationUser>(userRequestDto);

			user.Role = UserRole.User;
			user.UserName = user.Email;
			user.Image = userRequestDto.Image ?? string.Empty;
			user.BirthDate = (DateTime)(userRequestDto.BirthDate?.ToUniversalTime())!;

			var result = await _userManager.CreateAsync(user, userRequestDto.Password!);

			if (result.Succeeded)
			{
				await _userManager.AddToRoleAsync(user, UserRole.User.ToString());

				var userResponseDto = _mapper.Map<ApplicationUserResponseDto>(user);
				return ServiceResult<ApplicationUserResponseDto>.SuccessResult(userResponseDto);
			}
			else
			{
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				return ServiceResult<ApplicationUserResponseDto>.FailureResult(errors);
			}
		}

		// ID ile kullanıcıyı getirme
		public async Task<ServiceResult<ApplicationUserResponseDto>> GetUserByIdAsync(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return ServiceResult<ApplicationUserResponseDto>.FailureResult("Kullanıcı bulunamadı.");
			}

			var userResponseDto = _mapper.Map<ApplicationUserResponseDto>(user);
			return ServiceResult<ApplicationUserResponseDto>.SuccessResult(userResponseDto);
		}

		// Kullanıcı güncelleme
		public async Task<ServiceResult<ApplicationUserResponseDto>> UpdateUserAsync(string id, ApplicationUserRequestDto updatedUserDto)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return ServiceResult<ApplicationUserResponseDto>.FailureResult("Kullanıcı bulunamadı.");
			}

			user.FirstName = updatedUserDto.FirstName!;
			user.LastName = updatedUserDto.LastName!;
			user.Image = updatedUserDto.Image ?? string.Empty;
			user.UpdatedDate = DateTime.UtcNow;

			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				var userResponseDto = _mapper.Map<ApplicationUserResponseDto>(user);
				return ServiceResult<ApplicationUserResponseDto>.SuccessResult(userResponseDto);
			}
			else
			{
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				return ServiceResult<ApplicationUserResponseDto>.FailureResult(errors);
			}
		}

		// Kullanıcı silme (soft delete)
		public async Task<ServiceResult<string>> DeleteUserAsync(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null)
			{
				return ServiceResult<string>.FailureResult("Kullanıcı bulunamadı.");
			}
			if (user.Role == UserRole.Admin)
			{
				return ServiceResult<string>.FailureResult("Admin Silinemez.");
			}

			// Kullanıcıya ait içerikleri yorumlarla birlikte getir
			var contents = await _repositoryManager.ContentRepository.GetContentsByUserId(user.Id);

			if (contents != null && contents.Any())
			{
				// İçeriklerin ve bağlı yorumların toplu olarak soft delete yapılması
				var contentIds = contents.Select(c => c.Id).ToList();

				// İçeriklere bağlı tüm yorumları toplu soft delete
				await _repositoryManager.CommentRepository.SoftDeleteCommentsByContentIdsAsync(contentIds);

				// İçeriklerin kendilerini toplu soft delete
				await _repositoryManager.ContentRepository.SoftDeleteContentsByIdsAsync(contentIds);
			}

			// Kullanıcıyı soft delete yap
			user.State = State.Deleted;
			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				return ServiceResult<string>.SuccessMessageResult("Kullanıcı başarıyla silindi.");
			}
			else
			{
				var errors = string.Join(", ", result.Errors.Select(e => e.Description));
				return ServiceResult<string>.FailureResult(errors);
			}
		}




		// Eğitmen Rolünü Atama
		public async Task<ServiceResult<string>> ActivateInstructorRole(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (user == null)
			{
				return ServiceResult<string>.FailureResult("Kullanıcı Bulunamadı");
			}

			var userRoles = await _userManager.GetRolesAsync(user);

			// Admin rolünde olup olmadığını kontrol et
			if (userRoles.Contains(UserRole.Admin.ToString()))
			{
				return ServiceResult<string>.FailureResult("Admin Rol Değiştirilemez");
			}
			// Zaten eğitmen rolünde olup olmadığını kontrol et kontrol et
			if (userRoles.Contains(UserRole.UserAndInstructor.ToString()))
			{
				return ServiceResult<string>.FailureResult("Zaten Eğitmensiniz");
			}

			// Kullanıcının tüm mevcut rollerini kaldır
			foreach (var role in userRoles)
			{
				await _userManager.RemoveFromRoleAsync(user, role);
			}

			// Kullanıcıya yeni rolü ekle
			user.Role=UserRole.UserAndInstructor;
			await _userManager.UpdateAsync(user);

			var result = await _userManager.AddToRoleAsync(user, UserRole.UserAndInstructor.ToString());

			if (!result.Succeeded)
			{
				return ServiceResult<string>.FailureResult("Rol atanırken bir hata oluştu!");
			}

			return ServiceResult<string>.SuccessResult("Eğitmen Rolü Aktifleştirildi...");
		}

		public async Task<ServiceResult<UserAnalysisResponseDto>> GetAllUsersAnalysisAsync()
		{
			// Identity'den tüm kullanıcıları al
			var users = await _userManager.Users.ToListAsync();

			// Kullanıcıları durumlarına göre say
			int activeUserCount = users.Count(u => u.State == State.Active && u.Role!=UserRole.Admin);
			int deletedUserCount = users.Count(u => u.State == State.Deleted);

			// Admin kullanıcıları filtrele ve rollere göre say
			int regularUserCount = users.Count(u => u.Role == UserRole.User && u.State != State.Deleted);
			int userAndInstructorCount = users.Count(u => u.Role == UserRole.UserAndInstructor && u.State != State.Deleted);

			// Tüm gerekli sayımlarla birlikte response DTO oluştur
			var analysisResult = new UserAnalysisResponseDto
			{
				ActiveUsers = activeUserCount,
				DeletedUsers = deletedUserCount,
				RegularUsers = regularUserCount,
				UserAndInstructors = userAndInstructorCount
			};

			return ServiceResult<UserAnalysisResponseDto>.SuccessResult(analysisResult);
		}

		public async Task<ServiceResult<UserFilterResponseDto>> FilterUsersAsync(UserFilterRequestDto filterRequest)
		{
			// Tüm kullanıcıları Identity'den al
			var query = _userManager.Users
							.Include(u => u.Comments!.Where(c => c.State != State.Deleted)) 
							.Where(u => u.Role != UserRole.Admin && u.State != State.Deleted)
							.AsQueryable();


			// Arama filtresi (FirstName veya LastName'e göre arama yap)
			if (!string.IsNullOrEmpty(filterRequest.SearchTerm))
			{
				var searchTerm = filterRequest.SearchTerm.ToLower();
				query = query.Where(u => u.FirstName.ToLower().Contains(searchTerm) || u.LastName.ToLower().Contains(searchTerm));
			}

			// Rol filtresi
			if (filterRequest.Role.HasValue)
			{
				query = query.Where(u => u.Role == filterRequest.Role.Value);
			}

			// Toplam kullanıcı sayısını al
			int totalUsers = await query.CountAsync();

			// Sayfalama işlemi
			var users = await query
				.Skip((filterRequest.PageNumber - 1) * filterRequest.PageSize)
				.Take(filterRequest.PageSize)
				.ToListAsync();

			// Kullanıcıları DTO'ya dönüştür
			var userDtos = users.Select(_mapper.Map<ApplicationUserResponseDto>).ToList();

			// Toplam sayfa sayısını hesapla
			int totalPages = (int)Math.Ceiling(totalUsers / (double)filterRequest.PageSize);

			// Yanıt DTO'sunu oluştur
			var response = new UserFilterResponseDto
			{
				Users = userDtos,
				TotalPages = totalPages
			};

			return ServiceResult<UserFilterResponseDto>.SuccessResult(response);
		}






	}
}
