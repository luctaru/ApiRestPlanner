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
    public class UsersController : ControllerBase
    {
        private readonly UsersRepository usersRepository;

        public UsersController(IConfiguration configuration)
        {
            usersRepository = new UsersRepository(configuration);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = usersRepository.FindAll();
            return Ok(list);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            var model = usersRepository.FindByID(id);
            return Ok(model);
        }

        [HttpPost]
        public ActionResult Create([Bind("Name,CanCreatePlan,Removed")] Users users)
        {
            if (ModelState.IsValid)
            {
                usersRepository.Add(users);
                return RedirectToAction("Index");
            }

            return Ok(users);
        }

        //[HttpDelete("{id}")]
        //public ActionResult Delete([FromRoute]int id)
        //{
        //    var model = usersRepository.FindByID(id);
        //    if (model == null)
        //    {
        //        return StatusCode(StatusCodes.Status404NotFound);
        //    }
        //    usersRepository.Remove(id);
        //    return NoContent();
        //}

        [HttpPut]
        public ActionResult Edit([Bind("Id,Name,CanCreatePlan,Removed")]Users users)
        {
            var model = usersRepository.FindByID(users.Id);
            if (model == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            usersRepository.Update(users);
            return NoContent();
        }
    }
}