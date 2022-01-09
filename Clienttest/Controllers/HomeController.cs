using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Clienttest.Models;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Clienttest.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public IActionResult SendMessageToQueue(string email, string message)
        {
            try
            {
                var facrory = new ConnectionFactory
                {
                    Uri = new Uri("amqp://user:user@localhost:5672")
                };
                using var connection = facrory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare("queue1", true, false, false, null);
                var msg = new { Name = email, Message = message };
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(msg));
                channel.BasicPublish("", "queue1", null, body);

                return Content("email send to queue!");
            }
            catch (Exception e)
            {
                return Content($"error send message to queue: {e.Message}");
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
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
