
using Education.Entity.Models;

namespace Education.Data.Repositories.Abstract
{
	public interface IContentRepository : IRepositoryBase<Content, AppDbContext, long>
	{
		Task<IEnumerable<Content>> GetTopContentsAsync(int pageNumber, int pageSize);
		Task<IEnumerable<Content>> GetContentsByUserId(string userId);
		Task<int> GetTotalContentCountAsync();
		Task<(int TotalCourses, long TotalViews)> GetTotalCoursesAndViewsAsync();
		Task SoftDeleteContentsByIdsAsync(IEnumerable<long> contentIds);
	}

	
}
