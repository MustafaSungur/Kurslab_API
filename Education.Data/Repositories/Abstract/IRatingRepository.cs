
using Education.Entity.Models;

namespace Education.Data.Repositories.Abstract
{
	public interface IRatingRepository : IRepositoryBase<Rating, AppDbContext, long>
	{
		Task<bool> CheckRatingAsync(string userId, long contentId);
	}
	
}
