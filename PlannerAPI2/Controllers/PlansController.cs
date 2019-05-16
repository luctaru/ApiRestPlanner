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
    public class PlansController : ControllerBase
    {
        private readonly PlansRepository plansRepository;

        public PlansController(IConfiguration configuration)
        {
            plansRepository = new PlansRepository(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = plansRepository.FindAll();
            return Ok(list);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var model = plansRepository.FindByID(id);
            return Ok(model);
        }

        [HttpPost]
        public ActionResult Create([Bind("Name,Type,Users,Status,StartDate,EndDate,Description,Cost")] Plansend plans)
        {
            if (ModelState.IsValid)
            {
                plansRepository.Add(plans);
                return RedirectToAction("Index");
            }

            return NoContent();
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete([FromRoute]int id)
        //{
        //    var model = plansRepository.FindByID(id);
        //    if (model == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound);
        //    }
        //    plansRepository.Remove(id);
        //    return NoContent();
        //}

        [HttpPut]
        public ActionResult Edit([Bind("Id,Name,Type,Users,Status,StartDate,EndDate,Description,Cost")]Plansend plans)
        {
            var model = plansRepository.FindByID(plans.Id);
            if (model == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            plansRepository.Update(plans);
            return NoContent();
        }
    }
}