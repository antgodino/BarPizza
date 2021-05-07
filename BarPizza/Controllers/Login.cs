using BarPizza.Models;
using BarPizza.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BarPizza.Controllers
{
    public class Login : Controller
    {
        MyService myService;
        public Login(IMongoClient client)
        {

            myService = new MyService(client);
            var db = client.GetDatabase("pizzeria_bar");
        }
        // GET: Login
        public ActionResult Index()
        {
            return View("login");
        }

        // POST: Login/signin
        [HttpPost]
        public ActionResult signin(string username, string password)
        {
            User user = myService.getUser(username, password);
            if (user != null)
            {
                UserType usertype = myService.getUserType(user.user_type);
                HttpContext.Session.SetString("username", user.username);
                HttpContext.Session.SetString("name", user.name);
                HttpContext.Session.SetString("surname", user.surname);
                HttpContext.Session.SetString("role", usertype.code);
                if (usertype.code == "mgz")
                {
                    return RedirectToAction("Index", "Magazziniere");
                }
                else if (usertype.code == "pizza" || usertype.code == "bar")
                {
                    return RedirectToAction("Index", "Manager");
                }
            }
            ViewData["err_cred"] = "error";
            return View("login");
        }

        //Get
        public ActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

