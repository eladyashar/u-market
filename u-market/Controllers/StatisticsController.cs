using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using u_market.Models;
using u_market.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace u_market.Controllers
{
    [Authorize(Roles = "Admin,Client")]
    public class StatisticsController : Controller
    {
        private readonly MarketContext Ctx;
        public StatisticsController(MarketContext Ctx)
        {
            this.Ctx = Ctx;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            //return Ok(Logic.GetAll());
            return View();

        }
    }
}
