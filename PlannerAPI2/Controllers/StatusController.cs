using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlannerAPI2.Model;
using PlannerAPI2.Repositories;

namespace PlannerAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusRepository statusRepository;

        public StatusController(IConfiguration configuration)
        {
            statusRepository = new StatusRepository(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = statusRepository.FindAll();
            return Ok(list);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var model = statusRepository.FindByID(id);
            return Ok(model);
        }

        [HttpPost]
        public ActionResult Create([Bind("Name")] Planstatus status)
        {
            if (ModelState.IsValid)
            {
                statusRepository.Add(status);
                return RedirectToAction("Index");
            }

            return Ok(status);
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete([FromRoute]int id)
        //{
        //    var model = statusRepository.FindByID(id);
        //    if (model == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound);
        //    }
        //    statusRepository.Remove(id);
        //    return NoContent();
        //}

        [HttpPut]
        public ActionResult Edit([Bind("Id,Name")]Planstatus status)
        {
            var model = statusRepository.FindByID(status.Id);
            if (model == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            statusRepository.Update(status);
            return NoContent();
        }
    }
}