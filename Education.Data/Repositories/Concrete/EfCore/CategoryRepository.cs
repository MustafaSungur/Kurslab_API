using Education.Data.Repositories.Abstract;
using Education.Entity.Enums;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class CategoryRepository : RepositoryBase<Category, AppDbContext, int>, ICategoryRepository
	{
		public CategoryRepository(AppDbContext context) : base(context)
		{
		}

		public override IQueryable<Category> GetAll()
		{
			return _context.Categories
				.Include(c => c.ParentCategory)
				.Include(c => c.Contents)
				.Where(c => c.State != State.Deleted);
		}

		// Kategorilere göre eğitim sayısını ve izlenme oranlarını getir
		public async Task<List<CategoryStatistics>> GetCategoryStatisticsAsync()
		{
			var categoryStatistics = await _context.Categories
				.Where(c => c.State != State.Deleted)!
				.Include(c => c.Contents)!
				.ThenInclude(content => content.ViewedUsers) 
				.Select(category => new
				{
					CategoryName = category.Name,
					ParentId = category.ParentId,
					Contents = category.Contents!
						.Where(content => content.State != State.Deleted) 
				})
				.ToListAsync();

			var result = categoryStatistics
				.Select(c => new CategoryStatistics
				{
					CategoryName = c.CategoryName,
					CourseCount = c.Contents.Count(), // İçerik sayısını al
					TotalViews = c.Contents.Sum(content => content.ViewedUsers?.Count ?? 0) 
				})
				.ToList();

			return result;
		}


	}
}
