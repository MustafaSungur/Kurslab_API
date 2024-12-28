
using Education.Data.Repositories.Abstract;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class RatingRepository : RepositoryBase<Rating, AppDbContext, long>, IRatingRepository
	{
		public RatingRepository(AppDbContext context) : base(context)
		{
		}


	public async Task<bool> CheckRatingAsync(string userId, long contentId)
	{
		var rating = await _context.Ratings.FirstOrDefaultAsync(r => r.UserId == userId && r.ContentId == contentId);
		return rating != null;
	}

}
}
