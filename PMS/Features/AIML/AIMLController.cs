using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PMS.Attributes;
using PMS.Features.AIML.ViewModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PMS.Features.AIML
{

    [PmsAuthorize]
    public class AIMLController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AIMLController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

      
        public async Task<IActionResult> GetModels()
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "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");

            var response = await client.GetAsync("models");
            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
        }
        /// <summary>
        /// Handles the HTTP GET request to retrieve a response from an external AI service based on a predefined
        /// prompt.
        /// </summary>
        /// <remarks>This method sends a request to an external AI service to generate a response to a
        /// prompt about .NET. It uses an HTTP client to communicate with the service and returns the generated content
        /// as an HTTP OK result.</remarks>
        /// <returns>An <see cref="IActionResult"/> containing the AI-generated response text.</returns>
        public async Task<IActionResult> Index()
        {
            var prompt = "Tell me something about .NET";

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.groq.com/openai/v1/");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", "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"); // <-- replace

            var body = new
            {
                model = "groq/compound-mini", // ✅ Updated Model
                messages = new[]
                {
            new { role = "user", content = prompt }
        }
            };

            var content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("chat/completions", content);
            var result = await response.Content.ReadAsStringAsync();

            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(result);
            string text = data.choices[0].message.content.ToString();

            return Ok(text);
        }

    }
}
