using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GloboClimaFrontend.Services
{
    public interface IJwtService
    {
    Task<string> GetJwtTokenAsync(string userId);
    }

    public class JwtService : IJwtService
    {
        private readonly string _baseUrl;
        private readonly string _basicUsername;
        private readonly string _basicPassword;
        public JwtService(IConfiguration configuration)
        {
            _baseUrl = configuration["GlobalClimaAPI_URL"];
            _basicUsername = configuration["Jwt:Username"];
            _basicPassword = configuration["Jwt:Password"];
        }

        public async Task<string> GetJwtTokenAsync(string userId)
        {
            string EncryptSha256(string input)
            {
                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    var bytes = System.Text.Encoding.UTF8.GetBytes(input);
                    var hash = sha256.ComputeHash(bytes);
                    return string.Concat(hash.Select(b => b.ToString("x2")));
                }
            }

            var encryptedUsername = EncryptSha256(_basicUsername);
            var encryptedPassword = EncryptSha256(_basicPassword);
            var basicAuth = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"{encryptedUsername}:{encryptedPassword}"));
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
