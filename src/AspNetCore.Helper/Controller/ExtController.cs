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
        private readonly TRepository _repository;
        private readonly JsonSerializerOptions _serializerOptions;

        protected ExtController(TRepository repository)
        {
            _serializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.General);
            _repository = repository;
        }
        
        [HttpGet]
        public async Task<IResult> Get([FromQuery]DataSourceLoadOptions loadOptions) => Results.Json(await DataSourceLoader.LoadAsync(this._repository.GetAll(), loadOptions), _serializerOptions);
        
        
        [HttpGet("{id}")]
        public async Task<IResult> Get(string id)
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(entity);
        }
        
        [HttpPut("{id}")]
        public async Task<IResult> Put(string id, TEntity entity)
        {
            if (id != entity.Id)
            {
                return Results.BadRequest();
            }
            await _repository.Update(entity);
            return Results.NoContent();
        }
        
        [HttpPost]
        public async Task<IResult> Post(TEntity entity)
        {
            await _repository.Add(entity);
            return Results.NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IResult> Delete(string id)
        {
            await _repository.Delete(id);
            return Results.NoContent();
        }

    }
}
