using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace GloboClimaFrontend.Controllers
{
    public class ConsultController : Controller
    {
        private readonly string _baseUrl;
        private readonly Services.IJwtService _jwtService;

        public ConsultController(IConfiguration configuration, Services.IJwtService jwtService)
        {
            _baseUrl = configuration["GlobalClimaAPI_URL"];
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> ConsultWeatherCity(string cidade, string codigoPais)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("userCredentials");
            var password = HttpContext.Session.GetString("passwordCredentials");
            var token = await _jwtService.GetJwtTokenAsync(userId, username ?? "", password ?? "");

            var url = $"{_baseUrl}/api/weather/GetCityWeather?userId={userId}&cityName={cidade}&countryCode={codigoPais}";
            using var client = new HttpClient();
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            bool notFound = false;
            try
            {
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                // Verifica se o status HTTP é 404 ou se o JSON contém a chave "error"
                if ((int)response.StatusCode == 404 || (json != null && json.error != null))
                    notFound = true;
            }
            catch { }

            if (notFound)
            {
                ViewBag.WeatherResult = null;
                ViewBag.CityNotFound = true;
            }
            else
            {
                ViewBag.WeatherResult = result;
                ViewBag.CityNotFound = false;
            }
            ViewBag.Form = "ConsultarClimaCidade";
            return View("~/Views/Home/Index.cshtml");
        }
            [HttpGet]
        public IActionResult ConsultCountry()
        {
            ViewBag.Form = "ConsultarPais";
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> ConsultCountryPost(string nomePais)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("userCredentials");
            var password = HttpContext.Session.GetString("passwordCredentials");
            var token = await _jwtService.GetJwtTokenAsync(userId, username, password);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Login");
            }

            var url = $"{_baseUrl}/api/country/GetCountry?userId={userId}&countryName={nomePais}";
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            bool notFound = false;
            try
            {
                dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                if ((json != null && json.status == 404) || (json != null && json.title == "Not Found"))
                    notFound = true;

                if (json is Newtonsoft.Json.Linq.JArray && json.Count == 0)
                    notFound = true;
            }
            catch { }

            if (notFound)
            {
                ViewBag.CountryResult = null;
                ViewBag.CountryNotFound = true;
            }
            else
            {
                ViewBag.CountryResult = result;
                ViewBag.CountryNotFound = false;
            }
            ViewBag.Form = "ConsultarPais";
            return View("~/Views/Home/Index.cshtml");
        }
    }
}
