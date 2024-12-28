using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Education.Data.Repositories.Abstract
{
	public interface IRepositoryBase<TEntity, TContext, TId>
	  where TEntity : class
	  where TContext : DbContext
	{
		Task<TEntity> CreateAsync(TEntity entity);

		Task<bool> DeleteAsync(TId id);

		IQueryable<TEntity> GetAll();

		Task<TEntity?> GetByIdAsync(TId id);

		Task<TEntity> UpdateAsync(TEntity entity);

		IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression);




	}
}
