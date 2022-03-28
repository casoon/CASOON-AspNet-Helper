using System.Linq;
using AspNetCore.Extensions.Entity;
using AspNetCore.Extensions.Repository;
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
        private readonly TRepository _repository;

        public BaseController(TRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public ActionResult<IQueryable<TEntity>> Get()
        {
            return Ok(_repository.GetAll());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> Get(string id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            return entity;
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, TEntity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }
            await _repository.Update(entity);
            return NoContent();
        }
        
        [HttpPost]
        public async Task<ActionResult> Post(TEntity entity)
        {
            await _repository.Add(entity);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            await _repository.Delete(id);
            return NoContent();
        }

    }
}
