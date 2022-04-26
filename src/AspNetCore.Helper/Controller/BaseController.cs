using System.Linq;
using AspNetCore.Helper.Entity;
using AspNetCore.Helper.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Extensions.Controller {
    
    [Route("api/[controller]")]
    [ApiController]
    public abstract class BaseController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        public readonly TRepository Repository;

        protected BaseController(TRepository repository)
        {
            Repository = repository;
        }
        
        [HttpGet]
        public virtual ActionResult<IQueryable<TEntity>> Get()
        {
            return Ok(Repository.GetAll());
        }
        
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntity>> Get(string id)
        {
            var entity = await Repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }
        
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Put(string id, TEntity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }
            await Repository.Update(entity);
            return NoContent();
        }
        
        [HttpPost]
        public virtual async Task<ActionResult> Post(TEntity entity)
        {
            await Repository.Add(entity);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(string id)
        {
            await Repository.Delete(id);
            return NoContent();
        }

    }
}
