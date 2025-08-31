using System.IO;
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
        public IActionResult ConsultarClimaCidade(string? paisSelecionado = null)
        {
            ViewBag.Form = "ConsultarClimaCidade";
            ViewBag.PaisSelecionado = paisSelecionado; // Passar o país selecionado para a view
            return View("Index");
        }
    }
}
