using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository repository;

        public AppController(IMailService mailService, IDutchRepository repository)
        {
            this.mailService = mailService;
            this.repository = repository;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            //var results = this.context.Products.ToList();
            return View();
        }

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
                this.mailService.SendEmail(model.Email, model.Subject, $"Subject: {model.Subject} From: {model.Name} Email: {model.Email}");
                ViewBag.UserMessage = "Mail Sent!";
                ModelState.Clear();
            }

            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About us!";
            return View();
        }

        public IActionResult Shop()
        {
            var products = this.repository.GetAllProducts();
            return View(products);
        }
    }
}

