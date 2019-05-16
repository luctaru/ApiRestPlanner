using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlannerAPI2.Repositories;

namespace PlannerAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserhistoryController : ControllerBase
    {
        private readonly UserhistoryRepository userhistoryRepository;

        public UserhistoryController(IConfiguration configuration)
        {
            userhistoryRepository = new UserhistoryRepository(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = userhistoryRepository.FindAll();
            return Ok(list);

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var model = userhistoryRepository.FindByID(id);
            return Ok(model);
        }
    }
}