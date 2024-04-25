using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using veneer.Models;
using veneer.Services;
using veneer.Util;
using veneer.ViewModels;

namespace veneer.Controllers
{
    public class GenerateController : Controller
    {
        private readonly ILogger<GenerateController> _logger;
        private readonly FontService _fontService;
        private readonly IConfiguration _config;

        public GenerateController(ILogger<GenerateController> logger,
                                FontService fontService,
                                IConfiguration configuration)
        {
            _logger = logger;
            _fontService = fontService;
            _config = configuration;
        }

        [HttpGet]
        [Route("{tvshow}/create")]
        public ActionResult Create(string tvshow)
        {
            var imageDirs = _config.GetSection("ImageDirs");

            if (imageDirs.GetChildren().Any(item => item.Key == tvshow.ToLower()))
            {
                ViewBag.TvShow = tvshow.ApplyCase(LetterCasing.Title);
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: [Controller]/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("{tvshow}/create")]
        public ActionResult Create(string tvshow, Quote quote)
        {
            var imageDirs = _config.GetSection("ImageDirs");

            if (imageDirs.GetChildren().Any(item => item.Key == tvshow.ToLower()))
            {
                if (!ModelState.IsValid)
                {
                    RedirectToAction(nameof(Create));
                }

                var imageDir = imageDirs[tvshow.ToLower()];
                Trace.WriteLine(imageDir);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                string base64Image = ImageGenerator.Generate(imageDir, quote.Text, _fontService.Font);
                sw.Stop();

                ImageViewModel ivm = new ImageViewModel
                {
                    Text = quote.Text,
                    Base64Image = base64Image,
                    Duration = sw.ElapsedMilliseconds.ToString()
                };

                HttpContext.Session.SetString("ImageViewModel", JsonConvert.SerializeObject(ivm));
                return RedirectToAction(nameof(Result));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: [Controller]/Result
        [HttpGet]
        [Route("{tvshow}/result")]
        public ActionResult Result()
        {
            var ivmJson = HttpContext.Session.GetString("ImageViewModel");
            if (string.IsNullOrEmpty(ivmJson))
            {
                return RedirectToAction("Error");
            }

            ImageViewModel ivm = JsonConvert.DeserializeObject<ImageViewModel>(ivmJson);
            return View(ivm);
        }

        // GET: [Controller]/Random
        [HttpGet]
        [Route("{tvshow}/random")]
        public ActionResult Random(string tvshow)
        {
            var imageDirs = _config.GetSection("ImageDirs");

            if (imageDirs.GetChildren().Any(item => item.Key == tvshow.ToLower()))
            {
                var imageDir = imageDirs[tvshow.ToLower()];
                Trace.WriteLine(imageDir);

                Stopwatch sw = new Stopwatch();
                sw.Start();
                string base64Image = ImageGenerator.RandomImage(imageDir);
                sw.Stop();

                ImageViewModel ivm = new ImageViewModel
                {
                    Duration = sw.ElapsedMilliseconds.ToString(),
                    Base64Image = base64Image
                };

                return View(ivm);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
