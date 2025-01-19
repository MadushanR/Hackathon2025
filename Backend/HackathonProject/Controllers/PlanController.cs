using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text;
using HackathonProject.Models;
using System.Net.Http.Headers;

namespace HackathonProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlanController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public PlanController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;

            // Create or inject a shared HttpClient
            _httpClient = new HttpClient();
        }

        /// <summary>
        /// POST: api/plan/generate
        /// Generates an academic plan using OpenAI Chat Completion (gpt-3.5-turbo).
        /// </summary>
        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePlan([FromBody] PlanRequestDto dto)
        {
            // 1. Verify the student exists
            var student = await _context.Students.FindAsync(dto.StudentId);
            if (student == null)
            {
                return NotFound($"No student found with ID = {dto.StudentId}");
            }

            // 2. Create the prompt
            var prompt = $@"
A student has a current GPA of {dto.CurrentGPA} 
and wants a final GPA of {dto.DesiredGPA}.
They need {dto.TotalCreditsNeeded} more credits to graduate.
Please provide a recommended academic plan, including potential courses and
study strategies to help them achieve their desired GPA.";

            // 3. Call OpenAI chat completion
            string openAiResponse = await CallOpenAIChatAsync(prompt);

            // 4. Return the AI-generated text
            return Ok(new { planText = openAiResponse });
        }

        /// <summary>
        /// Helper method to call OpenAI Chat Completion using gpt-3.5-turbo.
        /// </summary>
        private async Task<string> CallOpenAIChatAsync(string userPrompt)
        {
            // Retrieve API key from config
            string apiKey = _config["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
            {
                return "OpenAI API key not configured.";
            }

            // Prepare the chat request for gpt-3.5-turbo
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    // You can customize the "system" message if you want
                    new { role = "system", content = "You are a helpful academic advisor." },
                    new { role = "user", content = userPrompt }
                },
                max_tokens = 300,
                temperature = 0.7
            };

            // Serialize to JSON
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            // Set authorization header
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            // POST to chat completion endpoint
            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                jsonContent
            );
            var responseString = await response.Content.ReadAsStringAsync();

            // If not success, return the error text
            if (!response.IsSuccessStatusCode)
            {
                return $"Error calling OpenAI API: {response.StatusCode}, {responseString}";
            }

            try
            {
                // Parse the JSON to extract the assistant's reply
                using var doc = JsonDocument.Parse(responseString);
                var root = doc.RootElement;
                var choices = root.GetProperty("choices");

                // The text is usually at `choices[0].message.content`
                var content = choices[0].GetProperty("message").GetProperty("content").GetString();
                return content.Trim();
            }
            catch (Exception ex)
            {
                return $"Error parsing OpenAI response: {ex.Message}";
            }
        }
    }

    /// <summary>
    /// DTO for plan generation request.
    /// </summary>
    public class PlanRequestDto
    {
        public int StudentId { get; set; }
        public double CurrentGPA { get; set; }
        public double DesiredGPA { get; set; }
        public int TotalCreditsNeeded { get; set; }
    }
}

