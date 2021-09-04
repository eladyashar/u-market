using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using u_market.Exceptions;

namespace u_market.Controllers.Stores
{
    [Authorize(Roles = "Admin,Client")]
    public class StoreController : Controller
    {
        private readonly ProductLogic ProductLogic;
        private readonly StoreLogic StoreLogic;

        public StoreController(MarketContext ctx)
        {
            ProductLogic = new ProductLogic(ctx);
            StoreLogic = new StoreLogic(ctx);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string? query)
        {
            if (User.IsInRole("Admin"))
            {
                return Ok(StoreLogic.GetAll(query));
            }

            return Ok(new []{ StoreLogic.FindMyStore(User.Claims.SingleOrDefault(c => c.Type.Equals("Username")).Value) });
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Store store)
        {
            if (StoreLogic.FindMyStore(User.Claims.SingleOrDefault(c => c.Type.Equals("Username")).Value) == null)
            {
                try
                {
                    StoreLogic.Insert(store, User);

                    return Ok();
                }
                catch (ModelValidationException ex)
                {
                    return BadRequest(new { ex.Message });
                }
                catch (Exception)
                {
                    return BadRequest(new { Message = "Something went wrong" });
                }
            }

            return BadRequest(new { Message = "User owns a store" });
        }

        [HttpPut]
        public IActionResult Update([FromBody] Store store)
        {
            try
            {
                StoreLogic.Update(store);
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
        public IActionResult Delete([FromBody] int storeId)
        {
            try
            {
                StoreLogic.Delete(storeId);
            }
            catch
            {
                return BadRequest(new { Message = "Something went wrong" });
            }

            return Ok();
        }

        [HttpPut]
        public IActionResult AddProduct([FromBody] JObject jsonData)
        {
            try
            {
                var product = jsonData["product"].ToObject<Product>();
                var tags = jsonData["tags"].ToObject<List<Tag>>();
                ProductLogic.AddProduct(product, tags);

                return Ok();
            }
            catch (ModelValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went wrong" });
            }
        }

        public IActionResult UpdateProduct([FromBody] JObject jsonData)
        {
            try
            {
                var product = jsonData["product"].ToObject<Product>();
                var tags = jsonData["tags"].ToObject<List<Tag>>();
                ProductLogic.UpdateProduct(product, tags);
                
                return Ok();
            }
            catch (ModelValidationException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { Message = "Something went wrong" });
            }
        }

        [HttpPut]
        public IActionResult RemoveProduct([FromBody] int productId)
        {
            try
            {
                ProductLogic.RemoveProduct(productId);
                
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}