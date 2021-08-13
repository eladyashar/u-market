using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;
using Microsoft.EntityFrameworkCore;
using u_market.Controllers.Users;

namespace u_market.Controllers.Stores
{
    public class StoreController : Controller
    {
        private readonly ProductLogic ProductLogic;
        private readonly StoreLogic StoreLogic;
        private readonly UsersLogic UsersLogic;

        public StoreController(MarketContext ctx)
        {
            ProductLogic = new ProductLogic(ctx);
            StoreLogic = new StoreLogic(ctx);
            UsersLogic = new UsersLogic(ctx);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(StoreLogic.GetAll());
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Store store)
        {
            StoreLogic.Insert(store, User);

            return Ok();
        }

        [HttpPut]
        public IActionResult Update([FromBody] Store store)
        {
            StoreLogic.Update(store);
            
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] int storeId)
        {
            StoreLogic.Delete(storeId);

            return Ok();
        }

        public IActionResult AddProduct([Bind("Name,StoreId,Price,ImageUrl,Description")] Product product)
        {
            try
            {
                ProductLogic.AddProduct(product);
                
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult UpdateProduct([Bind("Id,Name,StoreId,Price,ImageUrl,Description")] Product product)
        {
            try
            {
                ProductLogic.UpdateProduct(product);
                
                return StatusCode(200);
            }
            catch
            {
                return StatusCode(500);
            }
        }

        public IActionResult RemoveProduct([Bind("productId")] int productId)
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