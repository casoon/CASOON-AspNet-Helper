using System.Linq;
using AspNetCore.Helper.Entity;
using AspNetCore.Helper.Repository;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore.Helper.Controller {
    
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ExtController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository _repository;

        protected ExtController(TRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public async Task<string> Get([FromQuery]DataSourceLoadOptions loadOptions) => JsonSerializer.Serialize(await DataSourceLoader.LoadAsync(this._repository.GetAll(), loadOptions));
        
        
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
