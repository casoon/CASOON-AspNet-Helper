using System.Linq;
using AspNetCore.Helper.Entity;
using AspNetCore.Helper.Repository;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace AspNetCore.Helper.Controller {
    
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ExtController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        public readonly TRepository Repository;

        protected ExtController(TRepository repository)
        {
            Repository = repository;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]DataSourceLoadOptions loadOptions) => Ok(await DataSourceLoader.LoadAsync(this.Repository.GetAll(), loadOptions));
        
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var entity = await Repository.GetById(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(entity);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, TEntity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }
            await Repository.Update(entity);
            return NoContent();
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(TEntity entity)
        {
            await Repository.Add(entity);
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await Repository.Delete(id);
            return NoContent();
        }

    }
}
