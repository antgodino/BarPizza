using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Bson;
using BarPizza.Models;
using System.Diagnostics;
using BarPizza.Service;

namespace BarPizza.Controllers
{
    public class Manager : Controller
    {   
        private MyService myService;
        public Manager(IMongoClient client)
        {
            myService = new MyService(client);
        }
        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString("role");

            Warehouse warehouse = myService.getWarehouseByCode(role);
            var products = myService.getMyProduct(warehouse.id);
            var warehouseRq = myService.getMyRequest(warehouse.id);

            ViewData["show"] = "products";
            ViewData["products"] = products;
            ViewData["rq_size"] = warehouseRq.Count;

            if (HttpContext.Session.GetString("role") != null && (role == "pizza" || role =="bar"))
            {
                return View("Manager/IndexManager");
            }
            return View("login");
        }

        public new IActionResult Request(string[] message)
        {
            string role = HttpContext.Session.GetString("role");
            Warehouse warehouse = myService.getWarehouseByCode(role);
            var warehouseRq = myService.getMyRequestPending(warehouse.id);

            ViewData["show"] = "request";
            ViewData["request"] = warehouseRq;
            ViewData["rq_size"] = warehouseRq.Count;
            if (message != null)
                ViewData["message"] = message;

            return View("Manager/IndexManager");
        }

        [HttpPost]
        public new IActionResult Request(string id, string operation, string token)
        {
            string[] message;

            //trovo la richiesta da accettare
            WarehouseRequest wrequest = myService.getWarehouseRequest(new ObjectId(id));

            if (token == wrequest.token)
            {
                if (myService.executeRequest(wrequest, operation, HttpContext.Session.GetString("username")))
                {
                    message = new string[] { "rischiesta approvate", "success" };
                }
                else
                {
                    message = new string[] { "rischiesta non approvata", "danger" };
                }
            }
            else
            {
                message = new string[] { "token errato", "danger" };
            }
            return RedirectToAction("Request", new { message = message });
        }
    }
}
