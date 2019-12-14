using AdvancedNetCoreApi.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AdvancedNetCoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class
    {
        private Storage<T> _storage;

        public BaseController(Storage<T> storage)
        {
            _storage = storage;
        }

        [HttpGet]
        public IEnumerable<T> Get()
        {
            return _storage.GetAll();
        }

        [HttpGet("{id}")]
        public T Get(Guid id)
        {
            return _storage.GetById(id);
        }

        [HttpPost]
        public void Post([FromBody]T value)
        {
            _storage.Add(Guid.NewGuid(), value);
        }
    }
}
