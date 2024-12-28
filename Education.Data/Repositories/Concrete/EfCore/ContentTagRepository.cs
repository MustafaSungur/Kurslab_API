
using Education.Data.Repositories.Abstract;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class ContentTagRepository : RepositoryBase<ContentTag, AppDbContext, int>, IContentTagRepository
	{
		public ContentTagRepository(AppDbContext context) : base(context)
		{

		}

		public Task<IEnumerable<ContentTag>> GetContentTagsByContentIdAsync(long contentId)
		{

			var contentTags =  _context.ContentTags
							.Where(x => x.ContentId == contentId).ToList();

			return Task.FromResult<IEnumerable<ContentTag>>(contentTags);
		}

		public async Task<bool> DeleteRange(ContentTag[] contentTags)
		{
			_context.ContentTags.RemoveRange(contentTags);
			var result = await _context.SaveChangesAsync();
			return result > 0; 
		}

	}
}
