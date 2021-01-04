using FaceApiWebMVC.Models;
using MassTransit;
using Messaging.InterfacesConstants.Commands;
using Messaging.InterfacesConstants.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FaceApiWebMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBusControl _busControl;

        public HomeController(ILogger<HomeController> logger, IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterOrder()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterOrder(OrderViewModel model)
        {
            MemoryStream memory = new MemoryStream();

            using var uploadedFile = model.File.OpenReadStream();
            await uploadedFile.CopyToAsync(memory);

            model.ImageData = memory.ToArray();
            model.ImageUrl = model.File.FileName;
            model.OrderId = Guid.NewGuid();

            var rabbitMqUri = new Uri(
                RabbitMqMassTransitConstants.RabbitMqURI +
                RabbitMqMassTransitConstants.RegisterOrderCommandQueue
                );

            var endpoint = await _busControl.GetSendEndpoint(rabbitMqUri);
            await endpoint.Send<IRegisterOrderCommand>(
                new
                {
                    model.OrderId,
                    model.UserEmail,
                    model.ImageData,
                    model.ImageUrl
                }
                );

            ViewData["OrderId"] = model.OrderId;
            return View("Thanks");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
