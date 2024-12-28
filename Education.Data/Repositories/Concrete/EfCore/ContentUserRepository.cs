using Education.Data.Repositories.Abstract;
using Education.Entity.Models;

namespace Education.Data.Repositories.Concrete.EfCore
{
    public class ContentUserRepository : RepositoryBase<ContentUser, AppDbContext, long>, IContentUserRepository
    {
        public ContentUserRepository(AppDbContext context) : base(context)
        {
        }
    }
}
