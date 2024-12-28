
using System.Linq;
using Education.Data.Repositories.Abstract;
using Education.Entity.Enums;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class CommentRepository : RepositoryBase<Comment, AppDbContext, long>,ICommentRepository
	{
		public CommentRepository(AppDbContext context) : base(context)
		{
			
		}

		public async Task SoftDeleteCommentsByContentIdsAsync(IEnumerable<long> contentIds)
		{
			// Yorumların toplu soft delete işlemi
			await _context.Comments
				.Where(c => contentIds.Contains(c.ContentId))
				.ForEachAsync(c => c.State = State.Deleted);

			// Değişiklikleri veritabanına kaydet
			await _context.SaveChangesAsync();
		}

		public override Task<Comment?> GetByIdAsync(long id)
		{
			return _context.Comments.Include(c => c.User).FirstAsync(c=>c.Id==id)!;
		}

		public override async Task<Comment> UpdateAsync(Comment entity)
		{
			_context.Comments.Update(entity); 
			await _context.SaveChangesAsync(); 

			return entity; 
		}

		public  async Task<Comment> CreateAsync(Comment entity)
		{
			_context.Comments.Add(entity);
			await _context.SaveChangesAsync();

			return entity;
		}
	}
}
