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
        public IActionResult LoginUserGet()
        {
            return View("LoginUser");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Usuario e senha sao obrigatorios.";
                return View();
            }

            if (string.IsNullOrEmpty(_baseUrl) || string.IsNullOrEmpty(_basicUsername) || string.IsNullOrEmpty(_basicPassword))
            {
                ViewBag.Error = "Erro de configuracao do sistema.";
                return View();
            }

            try
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
                    if (string.IsNullOrWhiteSpace(input))
                        return string.Empty;
                        
                    using (var sha256 = System.Security.Cryptography.SHA256.Create())
                    {
                        var bytes = System.Text.Encoding.UTF8.GetBytes(input.Trim());
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
                        var json = await response.Content.ReadAsStringAsync();
                        var userId = Newtonsoft.Json.Linq.JObject.Parse(json)["userId"]?.ToString();
                        var userCredentials = Newtonsoft.Json.Linq.JObject.Parse(json)["username"]?.ToString();
                        var passwordCredentials = Newtonsoft.Json.Linq.JObject.Parse(json)["password"]?.ToString();

                        if (!string.IsNullOrEmpty(userId))
                        {
                            HttpContext.Session.SetString("UserId", userId);
                            if (!string.IsNullOrEmpty(userCredentials))
                                HttpContext.Session.SetString("userCredentials", userCredentials);
                            if (!string.IsNullOrEmpty(passwordCredentials))
                                HttpContext.Session.SetString("passwordCredentials", passwordCredentials);
                        }
                        return RedirectToAction("Index", "Home");
                    case System.Net.HttpStatusCode.Unauthorized:
                        ViewBag.Error = "Usuario ou senha invalidos.";
                        return View();
                    case System.Net.HttpStatusCode.InternalServerError:
                        ViewBag.Error = "Erro interno do servidor.";
                        return View();
                    default:
                        ViewBag.Error = "Erro inesperado.";
                        return View();
                }
            }
            catch (HttpRequestException)
            {
                ViewBag.Error = "Erro de conexao com o servidor.";
                return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Erro interno da aplicacao.";
                return View();
            }
        }

        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            // Limpar todas as informações da sessão
            HttpContext.Session.Clear();
            
            // Redirecionar para a tela de login
            return Redirect("/login");
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.RegisterError = "Usuário e senha são obrigatórios.";
                return View("LoginUser");
            }

            if (string.IsNullOrEmpty(_baseUrl) || string.IsNullOrEmpty(_basicUsername) || string.IsNullOrEmpty(_basicPassword))
            {
                ViewBag.RegisterError = "Erro de configuração do sistema.";
                return View("LoginUser");
            }

            try
            {
                using var client = new HttpClient();
                var basicAuth = Convert.ToBase64String(
                    System.Text.Encoding.UTF8.GetBytes($"{_basicUsername}:{_basicPassword}")
                );
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuth);

                var url = $"{_baseUrl}/api/auth/CreateUser";
                var body = new
                {
                    username = username,
                    password = password
                };
                var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(body);
                var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Parse da resposta JSON do backend
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var createdUser = Newtonsoft.Json.Linq.JObject.Parse(responseJson);
                    var userId = createdUser["userId"]?.ToString();
                    var encryptedUsername = createdUser["username"]?.ToString();
                    var encryptedPassword = createdUser["password"]?.ToString();
                    
                    ViewBag.RegisterSuccess = $"Usuário registrado com sucesso! Faça login.";
                    return View("LoginUser");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
                {
                    ViewBag.RegisterError = "Este nome de usuário já existe. Escolha outro nome.";
                    return View("LoginUser");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    ViewBag.RegisterError = "Não autorizado. Verifique as credenciais de configuração.";
                    return View("LoginUser");
                }
                else
                {
                    ViewBag.RegisterError = "Erro inesperado ao registrar usuário.";
                    return View("LoginUser");
                }
            }
            catch (HttpRequestException)
            {
                ViewBag.RegisterError = "Erro de conexão com o servidor.";
                return View("LoginUser");
            }
            catch (Exception)
            {
                ViewBag.RegisterError = "Erro interno da aplicação.";
                return View("LoginUser");
            }
        }
    }
}
