using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index() => View();

        [HttpGet("contact")] // makes Contact view discoverable at root of site
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            else
            {

            }

            return View(model);
        }

        public IActionResult About()
        {
            ViewBag.Title = "About us!";
            return View();
        }
    }
}

