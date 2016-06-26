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

            var topOne = output.Results.First().Title;

            var reply = string.Format("There are many options but aye recommend {0}", topOne);

            TextToSpeechService.Speak(reply);

            return View(output.Results);
        }

        [HttpPost]
        public ActionResult PostRecordedAudioVideo()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + @"uploads\";

            foreach (string upload in Request.Files)
            {
                var file = Request.Files[upload];
                if (file == null) continue;

                file.SaveAs(Path.Combine(path, Request.Form[0]));
            }

            var wavFileLocation = string.Format(path + @"output.wav");

            var response = SpeechToTextService.Interupt(wavFileLocation);
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var result = serializer.Deserialize<CogResponse>(response);

            var items = result.Results.First().Lexical.Split(' ').ToList();

            var output = RecipeLookupService.Query(items);

            var topOne = output.Results.First().Title;

            var reply = string.Format("There are many options but aye recommend {0}", topOne);

            TextToSpeechService.Speak(reply);

            return View(output.Results);
        }

    }
}