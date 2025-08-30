using Microsoft.AspNetCore.Mvc;

namespace GloboClimaFrontend.Controllers
{
    public class HomeController : Controller
    {
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
