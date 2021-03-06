using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Exceptions;
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
            try
            {
                Logic.UpdateTag(tag);
            }
            catch (ModelValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went wrong" });
            }

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add([FromBody] Tag tag)
        {
            try
            {
                Logic.AddTag(tag);
            }
            catch (ModelValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went wrong" });
            }

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                Logic.DeleteById(id);
            } 
            catch
            {
                return BadRequest(new { Message = "Something went wrong" });
            }

            return Ok();
        }
    }
}
