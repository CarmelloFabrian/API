using API.Repository.Data;
using API.Repository.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<Entity, Repository, Key> : ControllerBase
        where Entity : class
        where Repository : IRepository<Entity, Key>
    {
        private readonly Repository repository;

        public BaseController(Repository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public ActionResult<Entity> Get()
        {
            var result = repository.Get();
            return Ok(result);
        }

        [HttpGet("{key}")]
        public ActionResult<Entity> Get(Key key)
        {
            var result = repository.Get(key);
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<Entity> Post(Entity key)
        {
            var result = repository.Insert(key);
            return Ok(result);
        }

        [HttpDelete]
        public ActionResult<Entity> Delete(Key key)
        {
            var result = repository.Delete(key);
            return Ok(result);
        }

        [HttpPut]
        public ActionResult<Entity> Put(Entity entity)
        {
            var result = repository.Update(entity);
            return Ok(result);
        }
    }
}
