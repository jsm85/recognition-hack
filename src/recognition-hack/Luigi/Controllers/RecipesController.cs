using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Luigi.Models;
using Luigi.Services;

namespace Luigi.Controllers
{
    public class RecipesController : Controller
    {
        // GET: Recipes
        public ActionResult Index()
        {
            var binDirectoryPath = Server.MapPath("~/bin");
            var wavFileLocation = string.Format(@"{0}\Assets\AudioFile.wav", binDirectoryPath);

            var response = SpeechToTextService.Interupt(wavFileLocation);
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = serializer.Deserialize<CogResponse>(response);

            var items = result.Results.First().Lexical.Split(' ').ToList();

            var output = RecipeLookupService.Query(items);

            return View(output.Results);
        }

        public ActionResult Details(int id)
        {
            return View(new Recipe());
        }
    }
}