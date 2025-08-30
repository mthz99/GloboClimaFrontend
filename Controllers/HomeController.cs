using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace GloboClimaFrontend.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("/diagnostic/session")]
        public IActionResult ShowSessionUserId()
        {
            var userId = HttpContext.Session.GetString("UserId");
            return Content($"UserId na sessão: {userId ?? "(null)"}");
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
