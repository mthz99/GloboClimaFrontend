using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GloboClimaFrontend.Services
{
    public interface IJwtService
    {
    Task<string> GetJwtTokenAsync(string userId, string userName, string userPassword);
    }

    public class JwtService : IJwtService
    {
        private readonly string _baseUrl;
        private readonly string _basicUsername;
        private readonly string _basicPassword;
        public JwtService(IConfiguration configuration)
        {
            _baseUrl = configuration["GlobalClimaAPI_URL"];
            _basicUsername = configuration["Jwt_Username"];
            _basicPassword = configuration["Jwt_Password"];
        }

        public async Task<string> GetJwtTokenAsync(string userId, string userName, string userPassword)
        {
            
            var basicAuth = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{userName}:{userPassword}"));
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basicAuth);
            var body = new { UserId = userId };
            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(body), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_baseUrl}/api/auth/login", content);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            var token = Newtonsoft.Json.Linq.JObject.Parse(json)["token"]?.ToString();
            return token;
        }
    }
}
