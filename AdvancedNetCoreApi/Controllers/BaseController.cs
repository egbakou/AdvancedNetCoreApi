using AdvancedNetCoreApi.Database;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AdvancedNetCoreApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController<T> : ControllerBase where T : class, IEntity
    {
        private IGenericRepository<T> _repository;

        public BaseController(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<T> Get()
        {
            return _repository.GetAll();
        }
    }
}
