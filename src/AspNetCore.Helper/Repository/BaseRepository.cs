#nullable enable
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AspNetCore.Extensions.Entity;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Extensions.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;

        protected BaseRepository(DbContext context) => this._context = context;

        public IQueryable<TEntity> GetAll() => this._context.Set<TEntity>().AsQueryable();

        public ValueTask<TEntity?> GetById(object id) => this._context.Set<TEntity>().FindAsync(id);

        public Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
            => this._context.Set<TEntity>().FirstOrDefaultAsync(predicate);

        public async Task Add(TEntity entity)
        {
            await this._context.Set<TEntity>().AddAsync(entity);
            await this._context.SaveChangesAsync();
        }

        public Task Update(TEntity entity)
        {
            this._context.Entry(entity).State = EntityState.Modified;
            return this._context.SaveChangesAsync();
        }


        public async Task Delete(object id)
        {
            var entityToDelete = await this.GetById(id);
            if (entityToDelete != null)
            {
                await this.Delete(entityToDelete);
            }
        }

        public Task Delete(TEntity? entity)
        {
            if (entity != null)
            {
                this._context.Set<TEntity>().Remove(entity);
            }
            return this._context.SaveChangesAsync();
        }

        public Task<int> CountAll() => this._context.Set<TEntity>().CountAsync();

        public Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate)
            => this._context.Set<TEntity>().CountAsync(predicate);


    }
}