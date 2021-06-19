using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers.Tags
{
    [Authorize(Roles = "Admin,Client")]
    public class TagController : Controller
    {
        private TagLogic Logic { get; }
        public TagController(MarketContext Ctx)
        {
            Logic = new TagLogic(Ctx);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Client")]
        public IActionResult GetAll(string? filter)
        {
            return Ok(Logic.GetAll(filter));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult Update([FromBody] Tag tag)
        {
            Logic.UpdateTag(tag);
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add([FromBody] Tag tag)
        {
            Logic.AddTag(tag);
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Logic.DeleteById(id);
            return Ok();
        }
    }
}
