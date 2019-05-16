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
    public class PlanhistoryController : ControllerBase
    {
        private readonly PlanhistoryRepository planhistoryRepository;

        public PlanhistoryController(IConfiguration configuration)
        {
            planhistoryRepository = new PlanhistoryRepository(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = planhistoryRepository.FindAll();
            return Ok(list);

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var model = planhistoryRepository.FindByID(id);
            return Ok(model);
        }

    }
}