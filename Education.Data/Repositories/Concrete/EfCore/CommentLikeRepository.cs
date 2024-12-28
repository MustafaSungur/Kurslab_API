
using Education.Data.Repositories.Abstract;
using Education.Entity.Models;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class CommentLikeRepository : RepositoryBase<CommentLike, AppDbContext, long>, ICommentLikeRepository
	{
		public CommentLikeRepository(AppDbContext context) : base(context)
		{
		}

		// CommentLike'da State olmadığı için DeleteAsync'i override ediyoruz
		public override async Task<bool> DeleteAsync(long id)
		{
			var entity = await _dbSet.FindAsync(id);
			if (entity == null)
			{
				return false;
			}

			_dbSet.Remove(entity); // CommentLike'da doğrudan silme işlemi
			await _context.SaveChangesAsync();
			return true;
		}

		
	}

}
