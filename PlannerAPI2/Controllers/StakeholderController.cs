using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlannerAPI;
using PlannerAPI.Repositories;
using PlannerAPI2.Model;
using PlannerAPI2.Repositories;

namespace PlannerAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StakeholderController : Controller
    {
        private readonly StakeholderRepository stakeholderRepository;

        public StakeholderController(IConfiguration configuration)
        {
            stakeholderRepository = new StakeholderRepository(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = stakeholderRepository.FindAll();
            return Ok(list);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var model = stakeholderRepository.FindByID(id);
            return Ok(model);
        }

        [HttpPost]
        public ActionResult Create([Bind("Plans,Users")] Stakeholdersend stake)
        {
            if (ModelState.IsValid)
            {
                stakeholderRepository.Add(stake);
                return RedirectToAction("Index");
            }

            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete([FromRoute]int id)
        //{
        //    var model = stakeholderRepository.FindByID(id);
        //    if (model == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound);
        //    }
        //    stakeholderRepository.Remove(id);
        //    return NoContent();
        //}

        [HttpPut]
        public ActionResult Edit([Bind("Id,Plans,Users")]Stakeholdersend stake)
        {
            var model = stakeholderRepository.FindByID(stake.Id);
            if (model == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            stakeholderRepository.Update(stake);
            return NoContent();
        }
    }
}