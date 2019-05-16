using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PlannerAPI;
using PlannerAPI.Repositories;

namespace PlannerAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly TypeRepository typeRepository;

        public TypeController(IConfiguration configuration)
        {
            typeRepository = new TypeRepository(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = typeRepository.FindAll();
            return Ok(list);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var model = typeRepository.FindByID(id);
            return Ok(model);
        }

        [HttpPost]
        public ActionResult Create([Bind("Name")] Plantype plantype)
        {
            if (ModelState.IsValid)
            {
                typeRepository.Add(plantype);
                return RedirectToAction("Index");
            }

            return Ok(plantype);
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete([FromRoute]int id)
        //{
        //    var model = typeRepository.FindByID(id);
        //    if (model == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound);
        //    }
        //    typeRepository.Remove(id);
        //    return NoContent();
        //}

        [HttpPut]
        public ActionResult Edit([Bind("Id,Name")]Plantype plantype)
        {
            var model = typeRepository.FindByID(plantype.Id);
            if (model == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            typeRepository.Update(plantype);
            return NoContent();
        }
    }
}