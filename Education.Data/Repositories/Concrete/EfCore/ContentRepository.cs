
using System.Linq;
using Education.Data.Repositories.Abstract;
using Education.Entity.DTOs.ContentDTO;
using Education.Entity.DTOs.ContentTagDTO;
using Education.Entity.Enums;
using Education.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class ContentRepository : RepositoryBase<Content, AppDbContext, long>, IContentRepository
	{
		public ContentRepository(AppDbContext context) : base(context)
		{
		}
	
		public async Task<IEnumerable<Content>> GetTopContentsAsync(int pageNumber, int pageSize)
		{
			var contents = await _context.Contents
			.Include(content => content.ContentTags)!
				.ThenInclude(contentTag => contentTag.Tag)
			.Include(content => content.CreatedUser)
			.Include(content => content.Category)
			.Include(content => content.ViewedUsers)
			.Where(content => content.State != State.Deleted)
			.Select(content => new
			{
				Content = content,
				RatingCount = content.Ratings!.Count(),
				Rating = content.Ratings!.Count != 0 ? content.Ratings.Average(r => r.RatingValue) : 0
			})
			.OrderByDescending(x => x.Rating)
			.ThenByDescending(x => x.RatingCount)
			.ThenByDescending(x => x.Content.CreatedDate)
			.Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

			// Geri döndürülen veriyi Content nesnelerine dönüştür
			return contents.Select(x => {
				x.Content.RatingCount = x.RatingCount;
				x.Content.Rating = (float)x.Rating;
				return x.Content;
			});

		}


		// Toplam içerik sayısını getir
		public async Task<int> GetTotalContentCountAsync()
		{
			return await _context.Contents
				.Where(content => content.State != State.Deleted)
				.CountAsync();
		}

		public override IQueryable<Content> GetAll()
		{
			return _context.Contents
				.Include(c => c.ContentTags)!
					.ThenInclude(contentTag => contentTag.Tag) 
				.Include(c => c.Category) 
				.Include(c => c.ViewedUsers) 
				.Include(c => c.CreatedUser) 
				.Include(c => c.Comments)!
					.ThenInclude(c => c.User) 
				.Where(c => c.State != State.Deleted) 
				.Select(c => new Content
				{
					Id = c.Id,
					Title = c.Title,
					Description = c.Description,
					VideoUrl = c.VideoUrl,
					ImageUrl = c.ImageUrl,
					UserId = c.UserId,
					CreatedUser = c.CreatedUser,
					// Yalnızca silinmemiş yorumları alıyoruz
					Comments = c.Comments!.Where(comment => comment.State != State.Deleted).ToList(),
					ContentTags = c.ContentTags,
					CategoryId = c.CategoryId,
					Category = c.Category,
					ViewedUsers = c.ViewedUsers,
					Duration = c.Duration,
					State = c.State,
					RatingCount = _context.Ratings.Count(r => r.ContentId == c.Id), // İlgili içerik için rating sayısı
					Rating = (float)(_context.Ratings
						.Where(r => r.ContentId == c.Id)
						.Average(r => (double?)r.RatingValue) ?? 0) // İlgili içerik için ortalama rating
				});
		}



		public override async Task<Content?> GetByIdAsync(long id)
		{
			var contentData = await _context.Contents
				.Where(c => c.Id == id && c.State!=State.Deleted)
				.Include(content => content.Category)			
				.Include(content => content.ContentTags)!
					.ThenInclude(contentTag => contentTag.Tag)
				.Include(content => content.CreatedUser)
				.Include(content=>content.ViewedUsers)
				.Select(c => new
				{
					Content = c,
					RatingCount = _context.Ratings.Count(r => r.ContentId == id), 
					RatingAverage = _context.Ratings
									.Where(r => r.ContentId == id)
									.Average(r => (double?)r.RatingValue) ?? 0     
				})
				.FirstOrDefaultAsync();

			// Check if the content was found
			if (contentData == null)
				return null;

			// Map calculated properties to the Content entity
			contentData.Content.RatingCount = contentData.RatingCount;
			contentData.Content.Rating = (float)contentData.RatingAverage;

			return contentData.Content;

		}

		public Task<IEnumerable<Content>> GetContentsByUserId(string userId)
		{
			var contents = _context.Contents
			.Include(c => c.ContentTags)!
				.ThenInclude(contentTag => contentTag.Tag)
			.Include(c => c.Category)
			.Include(c => c.ViewedUsers)
			.Include(c => c.CreatedUser)	
			.Where(c => c.State != State.Deleted && c.UserId==userId)
			.Select(c => new Content
			{
				Id = c.Id,
				Title = c.Title,
				Description = c.Description,
				VideoUrl = c.VideoUrl,
				ImageUrl = c.ImageUrl,
				UserId = c.UserId,
				CreatedUser = c.CreatedUser,
				Comments = c.Comments!.Where(comment => comment.State != State.Deleted).ToList(),
				ContentTags = c.ContentTags,
				CategoryId = c.CategoryId,
				Category = c.Category,
				ViewedUsers = c.ViewedUsers,
				Duration = c.Duration,
				State = c.State,
				RatingCount = _context.Ratings.Count(r => r.ContentId == c.Id),
				Rating = (float)(_context.Ratings
					.Where(r => r.ContentId == c.Id)
					.Average(r => (double?)r.RatingValue) ?? 0)
			}).ToList();

			return Task.FromResult<IEnumerable<Content>>(contents);


		}

		// Toplam eğitim sayısını ve toplam izlenme sayısını getir
		public async Task<(int TotalCourses, long TotalViews)> GetTotalCoursesAndViewsAsync()
		{
			int totalCourses = await _context.Contents.Where(c=>c.State!=State.Deleted).CountAsync();
			long totalViews = await _context.ContentUsers.CountAsync();

			return (totalCourses, totalViews);
		}


		public async Task SoftDeleteContentsByIdsAsync(IEnumerable<long> contentIds)
		{
			// İçeriklerin toplu soft delete işlemi
			await _context.Contents
				.Where(c => contentIds.Contains(c.Id))
				.ForEachAsync(c => c.State = State.Deleted);

			// Değişiklikleri veritabanına kaydet
			await _context.SaveChangesAsync();
		}

	}
}
