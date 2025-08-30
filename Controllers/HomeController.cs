using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace GloboClimaFrontend.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/diagnostic/views")]
        public IActionResult ListViewsFiles()
        {
            var viewsPath = Path.Combine(Directory.GetCurrentDirectory(), "Views");
            if (!Directory.Exists(viewsPath))
                return Content("Views directory not found.");
            var files = Directory.GetFiles(viewsPath, "*.*", SearchOption.AllDirectories);
            return Content(string.Join("\n", files));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConsultarClimaCidade()
        {
            ViewBag.Form = "ConsultarClimaCidade";
            return View("Index");
        }
    }
}
