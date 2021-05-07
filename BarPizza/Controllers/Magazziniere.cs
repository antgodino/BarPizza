using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using BarPizza.Models;
using System.Diagnostics;
using System.Globalization;
using BarPizza.Service;

namespace BarPizza.Controllers
{
    public class Magazziniere : Controller
    {
        private MyService myService;
        public Magazziniere(IMongoClient client)
        {
            myService = new MyService(client);
            var db = client.GetDatabase("pizzeria_bar");
        }
        public IActionResult Index(string[] message)
        {
            var products = myService.getAllProducts();

            ViewData["products"] = products;
            if (message != null)
                ViewData["message"] = message;

            if (HttpContext.Session.GetString("role") != null && HttpContext.Session.GetString("role") == "mgz")
            {
                return View("Magazziniere/IndexMagazziniere");
            }
            return View("login");
        }
        [Route("Magazziniere/createRequest/{id}")]
        [HttpGet]
        public IActionResult createRequest(string id)
        {
            Products product = myService.getProductById(new ObjectId(id));
            List<WarehouseOperations> operations = myService.getOperations();
            List<Warehouse> warehouses = myService.getWarehousesNot(product);

            ViewData["id"] = id;
            ViewData["product"] = product;
            ViewData["operations"] = operations;
            ViewData["warehouses"] = warehouses;
            return View("Magazziniere/CreateRequest");
        }

        [HttpPost]
        public IActionResult createRequest(string id, string operation, string fromWarehouse, string toWarehouse)
        {
            ObjectId _id = new ObjectId(id);
            ObjectId _operation = new ObjectId(operation);
            ObjectId _fromWarehouse = new ObjectId(fromWarehouse);
            ObjectId _toWarehouse = new ObjectId(toWarehouse);

            WarehouseRequest wrequest = new WarehouseRequest()
            {
                token = RandomString(6),
                pending = true,
                userRequest = myService.getUser(HttpContext.Session.GetString("username")).id,
                warehouseItem = _id,
                warehouseOperation = _operation,
                toWarehouse = _toWarehouse,
                fromWarehouse = _fromWarehouse,
                request_date = DateTime.Now,
            };

            string[] message;

            if (myService.insertWarehouseRequest(wrequest))
            {
                message = new string[] { "rischiesta inviata con successo", "success" };
            }
            else
            {
                message = new string[] { "rischiesta non creata", "danger" };
            }
            return RedirectToAction("Index", new { message = message });
        }


        private static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
