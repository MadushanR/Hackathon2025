using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using HackathonProject.Models;

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
            _httpClient = new HttpClient();
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GeneratePlan([FromBody] PlanRequestDto dto)
        {
            // 1. Verify student exists
            var student = await _context.Students.FindAsync(dto.StudentId);
            if (student == null)
                return NotFound($"No student found with ID = {dto.StudentId}");

            // 2. Fetch courses for student
            var courses = await _context.Courses
                .Where(c => c.StudentId == dto.StudentId)
                .ToListAsync();

            var coursesInfo = courses.Any()
                ? string.Join("\n", courses.Select(c => $"Course: {c.CourseName} (Semester: {c.Semester}, Credits: {c.Credits}, Current Grade: {c.Grade})"))
                : "No course information available.";

            // 3. Build strict JSON-only prompt
            var prompt = $@"
Return ONLY a valid JSON object starting with '{{' and ending with '}}'.
Do NOT include explanations, markdown, or code fences.
Use this JSON structure exactly:
{{
  ""courses"": [{{ ""courseName"": ""string"", ""recommendation"": ""string"" }}],
  ""gradeBreakdown"": [{{ ""courseName"": ""string"", ""tips"": ""string"" }}],
  ""timeManagement"": {{ ""recommendedSchedule"": {{ ""Monday"": ""string"", ""Tuesday"": ""string"", ""Wednesday"": ""string"", ""Thursday"": ""string"", ""Friday"": ""string"", ""Saturday"": ""string"", ""Sunday"": ""string"" }} }},
  ""studyMethods"": {{ ""recommendedTechniques"": [{{ ""name"": ""string"", ""description"": ""string"" }}] }},
  ""resources"": {{ ""campus"": ""string"", ""online"": ""string"" }},
  ""motivation"": ""string"",
  ""innovativeSuggestions"": ""string""
}}

Student Info:
- StudentId: {dto.StudentId}
- Current GPA: {dto.CurrentGPA}
- Desired GPA: {dto.DesiredGPA}
- Credits Remaining: {dto.TotalCreditsNeeded}
- Strengths: {dto.Strengths}
- Weaknesses: {dto.Weaknesses}

Courses:
{coursesInfo}
";

            // 4. Call OpenAI
            var openAiResponse = await CallOpenAIChatAsync(prompt);

            // 5. Clean response
            string cleanedResponse = openAiResponse.Trim();
            int start = cleanedResponse.IndexOf('{');
            int end = cleanedResponse.LastIndexOf('}');
            if (start == -1 || end == -1 || end <= start)
                return BadRequest(new { error = "Unable to locate JSON object in OpenAI response." });

            cleanedResponse = cleanedResponse.Substring(start, end - start + 1);

            // 6. Parse JSON
            try
            {
                var parsedJson = JsonSerializer.Deserialize<JsonElement>(cleanedResponse);
                return Ok(parsedJson);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Unable to parse JSON from OpenAI response: " + ex.Message });
            }
        }

        private async Task<string> CallOpenAIChatAsync(string userPrompt)
        {
            var apiKey = _config["OpenAI:ApiKey"];
            if (string.IsNullOrEmpty(apiKey))
                return "OpenAI API key not configured.";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You are a highly knowledgeable academic advisor who provides extremely detailed and actionable academic plans." },
                    new { role = "user", content = userPrompt }
                },
                max_completion_tokens = 1500
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", jsonContent);
            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return $"Error calling OpenAI API: {response.StatusCode}, {responseString}";
            }

            try
            {
                using var doc = JsonDocument.Parse(responseString);
                var choices = doc.RootElement.GetProperty("choices");
                var content = choices[0].GetProperty("message").GetProperty("content").GetString();
                return content?.Trim() ?? "";
            }
            catch (Exception ex)
            {
                return $"Error parsing OpenAI response: {ex.Message}";
            }
        }
    }

    public class PlanRequestDto
    {
        public int StudentId { get; set; }
        public double CurrentGPA { get; set; }
        public double DesiredGPA { get; set; }
        public int TotalCreditsNeeded { get; set; }
        public string Strengths { get; set; }
        public string Weaknesses { get; set; }
    }
}
