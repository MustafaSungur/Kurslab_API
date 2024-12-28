
using Education.Data.Repositories.Concrete.EfCore;
using Education.Entity.Models;

namespace Education.Data.Repositories.Abstract
{
    public interface IContentUserRepository: IRepositoryBase<ContentUser,AppDbContext,long>
	{
	}
}
