using System;
using System.IO;
using System.Net;
using System.Web.Mvc;
using Luigi.Authentication;
using Luigi.Models;


namespace Luigi.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}