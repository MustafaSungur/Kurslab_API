
using Education.Data.Repositories.Abstract;
using Education.Entity.Models;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class TagRepository : RepositoryBase<Tag, AppDbContext, int>, ITagRepository
	{
		public TagRepository(AppDbContext context) : base(context)
		{
			
		}

		
	}
}
