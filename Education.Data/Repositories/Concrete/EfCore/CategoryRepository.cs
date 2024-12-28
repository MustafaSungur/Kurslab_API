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
				.Where(c => c.State != State.Deleted)
				.Select(category => new
				{
					CategoryName = category.Name,
					ContentList = category.Contents != null
						? category.Contents.Where(content => content.State != State.Deleted).ToList()
						: new List<Content>(),
					SubCategories = _context.Categories.Where(sc => sc.ParentId == category.Id && sc.State != State.Deleted)
						.SelectMany(subCategory => subCategory.Contents!.Where(content => content.State != State.Deleted))
						.ToList()
				})
				.ToListAsync();

			var result = categoryStatistics
				.Select(c => new CategoryStatistics
				{
					CategoryName = c.CategoryName,
					CourseCount = c.ContentList.Count + c.SubCategories.Count,
					TotalViews = c.ContentList.Sum(content => content.ViewedUsers?.Count ?? 0) +
								 c.SubCategories.Sum(content => content.ViewedUsers?.Count ?? 0)
				})
				.ToList();

			return result;
		}
	}
}
