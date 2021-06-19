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
using Newtonsoft.Json;

namespace u_market.Controllers.Statistics
{
    [Authorize(Roles = "Admin,Client")]
    public class StatisticsController : Controller
    {
        private readonly MarketContext Ctx;
        private StatisticsLogic Logic;

        public StatisticsController(MarketContext context)
        {
            Ctx = context;
            Logic = new StatisticsLogic(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAll()
        {
            return Json(JsonConvert.SerializeObject(Logic.GetAll()));
        }

    }
}
