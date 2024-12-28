using Education.Data.Repositories.Abstract;
using Education.Entity.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Education.Data.Repositories.Concrete.EfCore
{
	public class RepositoryBase<TEntity, TContext, TId> : IRepositoryBase<TEntity, TContext, TId>
		where TEntity : class
		where TContext : DbContext
	{
		protected readonly TContext _context;
		protected readonly DbSet<TEntity> _dbSet;

		public RepositoryBase(TContext context)
		{
			_context = context;
			_dbSet = _context.Set<TEntity>(); // DbSet tanımlaması
		}

		// Entity oluşturma işlemi
		public virtual async Task<TEntity> CreateAsync(TEntity entity)
		{
			await _dbSet.AddAsync(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		// State Deleted olarak ayarlanıyor
		public virtual async Task<bool> DeleteAsync(TId id)
		{
			var entity = await _dbSet
				.Where(e => EF.Property<TId>(e, "Id")!.Equals(id)
						 && EF.Property<State>(e, "State") != State.Deleted)
				.FirstOrDefaultAsync();

			if (entity == null)
			{
				return false;
			}

			// Entity'nin State'ini Deleted olarak işaretle
			_context.Entry(entity).Property("State").CurrentValue = State.Deleted;
			await _context.SaveChangesAsync();
			return true;
		}

		// Tüm kayıtları getirme işlemi, Deleted olanları dışarıda bırakıyoruz
		public virtual IQueryable<TEntity> GetAll()
		{
			return _dbSet
				.Where(e => EF.Property<State>(e, "State") != State.Deleted); // Deleted olmayan kayıtlar
		}

		// ID'ye göre kayıt getirme işlemi, Deleted olanları dışarıda bırakıyoruz
		public virtual async Task<TEntity?> GetByIdAsync(TId id)
		{
			return await _dbSet
				.Where(e => EF.Property<TId>(e, "Id")!.Equals(id)
						 && EF.Property<State>(e, "State") != State.Deleted)
				.FirstOrDefaultAsync();
		}

		// Entity güncelleme işlemi
		public virtual async Task<TEntity> UpdateAsync(TEntity entity)
		{
			_dbSet.Update(entity);
			await _context.SaveChangesAsync();
			return entity;
		}

		// Dinamik sorgulama işlemi, Deleted olanları dışarıda bırakıyoruz
		public virtual IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
		{
			return _dbSet
				.Where(expression)
				.Where(e => EF.Property<State>(e, "State") != State.Deleted)
				.AsNoTracking();
		}
	}
}
