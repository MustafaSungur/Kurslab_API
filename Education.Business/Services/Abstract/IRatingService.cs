using Education.Business.Exeptions;
using Education.Entity.DTOs.RaitingDTO;

namespace Education.Business.Services.Abstract
{
	public interface IRatingService
	{
		Task<ServiceResult<RatingResponseDto>> CreateRatingAsync(RatingRequestDto ratingRequestDto);
	}
}
