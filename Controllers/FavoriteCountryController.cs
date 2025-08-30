using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace GloboClimaFrontend.Controllers
{
    public class FavoriteCountryController : Controller
    {
        private readonly string? _baseUrl;
        private readonly Services.IJwtService _jwtService;

        public FavoriteCountryController(Microsoft.Extensions.Configuration.IConfiguration configuration, Services.IJwtService jwtService)
        {
            _baseUrl = configuration["GlobalClimaAPI_URL"];
            _jwtService = jwtService;
        }

        [HttpPost]
        [Route("Favorites/AdicionarPais")]
        public async Task<IActionResult> AddCountryFavorite( string countryName, string countryCode)
        {
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserName");
            var userPassword = HttpContext.Session.GetString("UserPassword");
            var token = userId != null ? await _jwtService.GetJwtTokenAsync(userId, userName, userPassword) : null;
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_baseUrl) || string.IsNullOrEmpty(countryName) || string.IsNullOrEmpty(countryCode))
            {
                TempData["Error"] = "Informações insuficientes para adicionar país favorito.";
                return RedirectToAction("GetFavoriteCountryList");
            }

            var addUrl = $"{_baseUrl}/api/favorites/AddCountryFavorite";
            var bodyObj = new {
                UserId = userId,
                CountryName = countryName,
                CountryCode = countryCode
            };
            var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(bodyObj);
            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var content = new System.Net.Http.StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(addUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "País favorito adicionado com sucesso.";
                }
                else
                {
                    TempData["Error"] = "Erro ao adicionar país favorito.";
                }
            }
            return RedirectToAction("GetFavoriteCountryList");
        }


            [HttpGet]
            [Route("Favorites/Paises")]
            public async Task<IActionResult> GetFavoriteCountryList()
            {
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserName");
                var userPassword = HttpContext.Session.GetString("UserPassword");
                var token = userId != null ? await _jwtService.GetJwtTokenAsync(userId, userName, userPassword) : null;
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_baseUrl))
                {
                    return RedirectToAction("Login", "Login");
                }
                var url = $"{_baseUrl}/api/favorites/GetCountryFavorites?userId={userId}";
                List<FavoriteCountryViewModel> favoriteCountries = new();
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
                        string? nome = item["countryName"]?.ToString();
                        string? codigoPais = item["countryCode"]?.ToString();
                        DateTime dataCriacao = DateTime.MinValue;
                        var createdAtToken = item["createdAt"];
                        if (createdAtToken != null && DateTime.TryParse(createdAtToken.ToString(), out var dt))
                            dataCriacao = dt;
                        favoriteCountries.Add(new FavoriteCountryViewModel
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
                ViewBag.FavoriteCountries = favoriteCountries;
                ViewBag.Form = "FavoritesPaises";
                return View("~/Views/Home/Index.cshtml");
            }

            [HttpPost]
            [Route("Favorites/ApagarPais")]
            public async Task<IActionResult> DeleteCountryFavorite(string countryId)
            {
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserName");
                var userPassword = HttpContext.Session.GetString("UserPassword");
                var token = userId != null ? await _jwtService.GetJwtTokenAsync(userId, userName, userPassword) : null;
                if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(_baseUrl) || string.IsNullOrEmpty(countryId))
                {
                    TempData["Error"] = "País favorito não encontrado.";
                    return RedirectToAction("GetFavoriteCountryList");
                }
                var deleteUrl = $"{_baseUrl}/api/favorites/RemoveCountryFavorite?userId={userId}&countryId={countryId}";
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.DeleteAsync(deleteUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Success"] = "País favorito removido com sucesso.";
                    }
                    else
                    {
                        TempData["Error"] = "Erro ao remover país favorito.";
                    }
                }
                return RedirectToAction("GetFavoriteCountryList");
            }

            public class FavoriteCountryViewModel
            {
                public string? Id { get; set; }
                public string? Nome { get; set; }
                public string? CodigoPais { get; set; }
                public DateTime DataCriacao { get; set; }
            }
        }
}
