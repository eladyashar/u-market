﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using u_market.DAL;
using u_market.Models;

namespace u_market.Controllers
{
    public class ProductController : Controller
    {
        private readonly MarketContext Ctx;

        public ProductController(MarketContext Ctx)
        {
            this.Ctx = Ctx;
        }

        public ActionResult Index()
        {
            ViewBag.NumTimes = GetAll().Select(c => c.Name).ToList()[0];
            return View();
        }

        private List<Product> GetAll()
        {
            return Ctx.Products.ToList();
        }

        public ActionResult Welcome(string name, int numTimes = 1)
        {
            ViewBag.Message = "Hello " + name;
            ViewBag.NumTimes = numTimes;

            return View();
        }
    }
}
