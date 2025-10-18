using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Demo.PL.Controllers
{
    public class AiChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string? _apiKey;


        public AiChatController(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _apiKey = config["OpenAI:ApiKey"];
        }

        #region Index
        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Ask(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return Json(new { reply = "⚠️ Please type a message." });

            if (string.IsNullOrWhiteSpace(_apiKey))
                return Json(new { reply = "⚠️ OpenAI API key is missing from configuration." });

            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                var body = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = "You are an AI assistant for the Small Companies System (SCS)." },
                        new { role = "user", content = message }
                    }
                };

                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return Json(new { reply = $"❌ API error: {response.StatusCode} - {responseText}" });

                using var doc = JsonDocument.Parse(responseText);
                var reply = doc.RootElement.GetProperty("choices")[0]
                    .GetProperty("message").GetProperty("content").GetString();

                return Json(new { reply });
            }
            catch (Exception ex)
            {
                return Json(new { reply = $"⚠️ Exception: {ex.Message}" });
            }
        }
        #endregion
    }
}
