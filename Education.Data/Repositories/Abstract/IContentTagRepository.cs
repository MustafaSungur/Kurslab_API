
using Education.Entity.Models;

namespace Education.Data.Repositories.Abstract
{
	public interface IContentTagRepository : IRepositoryBase<ContentTag, AppDbContext, int>
	{
		Task<IEnumerable<ContentTag>> GetContentTagsByContentIdAsync(long contentId);
		Task<bool> DeleteRange(ContentTag[] contentTags);
	}
}
