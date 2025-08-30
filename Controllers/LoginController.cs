using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace GloboClimaFrontend.Controllers
{
    public class LoginController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _basicUsername;
        private readonly string _basicPassword;

        public LoginController(IConfiguration configuration)
        {
            _baseUrl = configuration["GlobalClimaAPI_URL"];
            _basicUsername = configuration["Jwt_Username"];
            _basicPassword = configuration["Jwt_Password"];
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("/login");
        }

        [Route("login")]
        public IActionResult LoginUser()
        {
            return View();
        }

        [Route("Login/Login")]
        public IActionResult LoginUserAlias()
        {
            return View("LoginUser");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(string username, string password)
        {
            using var client = new HttpClient();
            var basicAuth = Convert.ToBase64String(
                System.Text.Encoding.UTF8.GetBytes($"{_basicUsername}:{_basicPassword}")
            );
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuth);

            // Criptografar username e password usando SHA256
            string EncryptSha256(string input)
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                    var hash = sha256.ComputeHash(bytes);
                    return string.Concat(hash.Select(b => b.ToString("x2")));
                }
            }

            var encryptedUsername = EncryptSha256(username);
            var encryptedPassword = EncryptSha256(password);

            var url = $"{_baseUrl}/api/auth/LoginUser?username={encryptedUsername}&password={encryptedPassword}";
            var response = await client.GetAsync(url);

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    // Autenticado com sucesso
                    var json = await response.Content.ReadAsStringAsync();
                    var userId = Newtonsoft.Json.Linq.JObject.Parse(json)["userId"]?.ToString();
                    var userCredentials = Newtonsoft.Json.Linq.JObject.Parse(json)["username"]?.ToString();
                    var passwordCredentials = Newtonsoft.Json.Linq.JObject.Parse(json)["password"]?.ToString();

                    if (!string.IsNullOrEmpty(userId))
                    {
                        HttpContext.Session.SetString("UserId", userId);
                        HttpContext.Session.SetString("userCredentials", userCredentials);
                        HttpContext.Session.SetString("passwordCredentials", passwordCredentials);
                    }
                    return RedirectToAction("Index", "Home");
                case System.Net.HttpStatusCode.Unauthorized:
                    ViewBag.Error = "Usuário ou senha inválidos.";
                    return View();
                case System.Net.HttpStatusCode.InternalServerError:
                    ViewBag.Error = "Erro interno do servidor.";
                    return View();
                default:
                    ViewBag.Error = "Erro inesperado.";
                    return View();
            }
        }
    }
}
