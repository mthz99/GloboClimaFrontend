using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GloboClimaFrontend.Controllers
{
        public class FavoriteCitysController : Controller
        {
            private readonly string? _baseUrl;
            private readonly Services.IJwtService _jwtService;

            public FavoriteCitysController(Microsoft.Extensions.Configuration.IConfiguration configuration, Services.IJwtService jwtService)
            {
                _baseUrl = configuration["GlobalClimaAPI_URL"];
                _jwtService = jwtService;
            }

        [HttpPost]
        [Route("Favorites/AdicionarCidade")]
        public async Task<IActionResult> AddFavoriteCity(string cityName, string countryCode)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("userCredentials");
            var password = HttpContext.Session.GetString("passwordCredentials");
            var token = await _jwtService.GetJwtTokenAsync(userId, username, password);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_baseUrl))
            {
                TempData["Error"] = "Usuário não autenticado.";
                return RedirectToAction("GetFavoriteCityList");
            }

            var url = $"{_baseUrl}/api/favorites/AddCityFavorite";
            var body = new
            {
                UserId = userId,
                CityName = cityName,
                CountryCode = countryCode
            };
            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            using var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var content = new System.Net.Http.StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Cidade favorita adicionada com sucesso.";
            }
            else
            {
                TempData["Error"] = "Erro ao adicionar cidade favorita.";
            }
            return RedirectToAction("GetFavoriteCityList");
        }

        [HttpGet]
        [Route("Favorites/Cidades")]
        public async Task<IActionResult> GetFavoriteCityList()
        {
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("userCredentials");
            var password = HttpContext.Session.GetString("passwordCredentials");
            var token = await _jwtService.GetJwtTokenAsync(userId, username, password);

           if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_baseUrl))
            {
                TempData["Error"] = "Usuário não autenticado.";
                return RedirectToAction("GetFavoriteCityList");
            }

            var url = $"{_baseUrl}/api/favorites/GetCityFavorites?userId={userId}";
            List<FavoriteCityViewModel> favoriteCities = new();
            using var client = new System.Net.Http.HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            try
            {
                var jsonArray = Newtonsoft.Json.Linq.JArray.Parse(result);
                foreach (var item in jsonArray)
                {
                    string? id = item["id"]?.ToString();
                    string? nome = item["cityName"]?.ToString();
                    string? codigoPais = item["countryCode"]?.ToString();
                    DateTime dataCriacao = DateTime.MinValue;
                    var createdAtToken = item["createdAt"];
                    if (createdAtToken != null && DateTime.TryParse(createdAtToken.ToString(), out var dt))
                        dataCriacao = dt;
                    favoriteCities.Add(new FavoriteCityViewModel
                    {
                        Id = id,
                        Nome = nome,
                        CodigoPais = codigoPais,
                        DataCriacao = dataCriacao
                    });
                }
            }
            catch
            {
                // Se o resultado não for um array válido, retorna lista vazia
            }
            ViewBag.FavoriteCities = favoriteCities;
            ViewBag.Form = "FavoritesCidades";
            return View("~/Views/Home/Index.cshtml");
        }

        [HttpPost]
        [Route("Favorites/ApagarCidade")]
        public async Task<IActionResult> DeleteCityFavorite(string cityId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var username = HttpContext.Session.GetString("userCredentials");
            var password = HttpContext.Session.GetString("passwordCredentials");
            var token = await _jwtService.GetJwtTokenAsync(userId, username, password);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_baseUrl) || string.IsNullOrEmpty(cityId))
            {
                TempData["Error"] = "Cidade favorita não encontrada.";
                return RedirectToAction("GetFavoriteCityList");
            }
            // Chama o endpoint de remoção (DELETE)
            var deleteUrl = $"{_baseUrl}/api/favorites/RemoveCityFavorite?userId={userId}&cityId={cityId}";
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync(deleteUrl);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Cidade favorita removida com sucesso.";
                }
                else
                {
                    TempData["Error"] = "Erro ao remover cidade favorita.";
                }
            }
            return RedirectToAction("GetFavoriteCityList");
        }
    }

    public class FavoriteCityViewModel
    {
    public string? Id { get; set; }
    public string? Nome { get; set; }
    public string? CodigoPais { get; set; }
    public DateTime DataCriacao { get; set; }
    }
}
