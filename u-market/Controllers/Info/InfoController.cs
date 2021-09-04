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
    [Authorize(Roles = "Client")]
    public class InfoController : Controller
    {
        public InfoController(MarketContext Ctx)
        {
        }

        [Authorize(Roles = "Client")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
