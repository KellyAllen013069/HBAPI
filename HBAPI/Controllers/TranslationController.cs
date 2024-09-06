using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslationController : ControllerBase
    {
        private readonly ILogger<TranslationController> _logger;
        private readonly string _apiKey;

        public TranslationController(ILogger<TranslationController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiKey = configuration["GoogleTranslate:ApiKey"];  // Fetch API key from configuration (appsettings.json)
        }

        // POST: api/Translation/AppendTranslation
        [HttpPost("AppendTranslation")]
        public async Task<IActionResult> AppendTranslation([FromBody] TranslationRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.TextToTranslate) || string.IsNullOrWhiteSpace(request.TargetLanguage))
            {
                _logger.LogWarning("Invalid input data received in AppendTranslation");
                return BadRequest(new { success = false, message = "Invalid input data" });
            }

            try
            {
                _logger.LogInformation($"Received translation request: Key: {request.Key}, TextToTranslate: {request.TextToTranslate}, TargetLanguage: {request.TargetLanguage}");

                string translatedText = await TranslateTextAsync(request.TextToTranslate, request.TargetLanguage);
                await AppendTranslationToJsonAsync(request.Key, translatedText);

                return Ok(new { success = true, message = "Translation added successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AppendTranslation: {ex.Message}");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        // Function to translate text using Google Translate API
        private async Task<string> TranslateTextAsync(string text, string targetLang = "es")
        {
            using (HttpClient client = new HttpClient())
            {
                var url = $"https://translation.googleapis.com/language/translate/v2?key={_apiKey}";

                var content = new StringContent(
                    JsonConvert.SerializeObject(new { q = text, target = targetLang, format = "text" }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(url, content);
                string result = await response.Content.ReadAsStringAsync();

                var json = JObject.Parse(result);
                return json["data"]["translations"][0]["translatedText"].ToString();
            }
        }

        // Function to append translation to JSON file
        private async Task AppendTranslationToJsonAsync(string key, string translatedText)
        {
            try
            {
                string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "locales", "es.json");
                _logger.LogInformation($"Appending translation to JSON file at {jsonPath}");

                // Read the existing JSON file
                var jsonData = JsonConvert.DeserializeObject<JObject>(await System.IO.File.ReadAllTextAsync(jsonPath));

                // Append the new translation to the JSON object
                jsonData[key] = translatedText;

                // Write the updated JSON back to the file
                await System.IO.File.WriteAllTextAsync(jsonPath, JsonConvert.SerializeObject(jsonData, Formatting.Indented));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error appending translation to JSON file: {ex.Message}");
                throw;
            }
        }
    }

    // DTO for handling requests
    public class TranslationRequest
    {
        public string Key { get; set; }            // The key in the JSON file
        public string TextToTranslate { get; set; } // The text that needs to be translated
        public string TargetLanguage { get; set; }  // The target language (e.g., "es" for Spanish)
    }
}
