#nullable enable
using System.Threading.Tasks;
using AspNetCore.Extensions.Entity;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AspNetCore.Extensions.Repository
{
    public interface IRepository<TEntity>
    {
        Task Delete(TEntity? entity);
        Task Delete(object id);
        IQueryable<TEntity> GetAll();
        ValueTask<TEntity?> GetById(object id);
        Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        Task Add(TEntity entity);
        Task Update(TEntity entity);

        Task<int> CountAll();
        Task<int> CountWhere(Expression<Func<TEntity, bool>> predicate);
    }
}