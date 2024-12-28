
using Education.Entity.Models;

namespace Education.Data.Repositories.Abstract
{
	public interface ICategoryRepository:IRepositoryBase<Category,AppDbContext,int>
	{
		Task<List<CategoryStatistics>> GetCategoryStatisticsAsync();
	}
}
