using Application.IService;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Service
{
    public class TinyLlamaAIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TinyLlamaAIService> _logger;

        public string ServiceName => "TinyLlama AI Service";

        public TinyLlamaAIService(HttpClient httpClient, ILogger<TinyLlamaAIService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpClient.BaseAddress = new Uri("http://localhost:11434");
            _httpClient.Timeout = TimeSpan.FromMinutes(2); // تقليل الوقت
        }

        public async Task<string> GenerateAnswerAsync(string context, string question)
        {
            try
            {
                // التحقق من المدخلات
                if (string.IsNullOrWhiteSpace(context) || string.IsNullOrWhiteSpace(question))
                    return "Error: Context and question are required.";

                var prompt = $@"Based on the following context, please answer the question clearly and concisely.

CONTEXT:
{context}

QUESTION:
{question}

ANSWER:";

                var requestData = new
                {
                    model = "tinyllama",
                    prompt = prompt,
                    stream = false,
                    options = new
                    {
                        temperature = 0.3,  // زيادة طفيفة لتحسين الإبداع
                        top_p = 0.9,
                        top_k = 40,
                        num_predict = 200,  // زيادة الطول قليلاً
                        stop = new[] { "\n\n", "###", "QUESTION:" }  // إضافة stop sequences
                    }
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending request to TinyLlama for question: {Question}", question);

                var response = await _httpClient.PostAsync("/api/generate", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API Error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                    return $"API Error: {response.StatusCode} - {errorContent}";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OllamaResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var cleanedResponse = CleanResponse(result?.Response);
                _logger.LogInformation("Received response from TinyLlama: {Response}", cleanedResponse);

                return cleanedResponse ?? "No response generated";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GenerateAnswerAsync");
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> SummarizeTextAsync(string text)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                    return "Error: Text is required.";

                var prompt = $@"Please provide a concise summary of the following text. Focus on the main ideas and key points:

TEXT:
{text}

SUMMARY:";

                var requestData = new
                {
                    model = "tinyllama",
                    prompt = prompt,
                    stream = false,
                    options = new
                    {
                        temperature = 0.2,  // أقل temperature للتلخيص
                        top_p = 0.9,
                        num_predict = 150,
                        stop = new[] { "\n\n", "###", "TEXT:" }
                    }
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending summarization request to TinyLlama for text of length: {Length}", text.Length);

                var response = await _httpClient.PostAsync("/api/generate", content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Summarization API Error: {StatusCode} - {Content}", response.StatusCode, errorContent);
                    return $"API Error: {response.StatusCode} - {errorContent}";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OllamaResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var cleanedResponse = CleanResponse(result?.Response);
                _logger.LogInformation("Received summary from TinyLlama: {Summary}", cleanedResponse);

                return cleanedResponse ?? "No summary generated";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SummarizeTextAsync");
                return $"Error: {ex.Message}";
            }
        }

        private string CleanResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
                return response;

            // تنظيف أكثر تقدمًا
            var cleaned = response.Trim();

            // إزالة أي تكرار للprompt
            cleaned = Regex.Replace(cleaned, @"(CONTEXT:|QUESTION:|ANSWER:|TEXT:|SUMMARY:).*$", "", RegexOptions.Singleline);

            // إزالة الأسطر الفارغة المتعددة
            cleaned = Regex.Replace(cleaned, @"\n\s*\n", "\n");

            // إزالة المسافات الزائدة
            cleaned = Regex.Replace(cleaned, @"\s+", " ");

            return cleaned.Trim();
        }

        public async Task<bool> IsServiceAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/tags");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("TinyLlama service is available");
                    return true;
                }

                _logger.LogWarning("TinyLlama service returned status: {StatusCode}", response.StatusCode);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TinyLlama service is not available");
                return false;
            }
        }

   
        public async Task<string> GetModelInfoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/tags");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return content;
                }
                return "Unable to get model info";
            }
            catch (Exception ex)
            {
                return $"Error getting model info: {ex.Message}";
            }
        }
    }

    public class OllamaResponse
    {
        public string Model { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Response { get; set; }
        public bool Done { get; set; }
        public string[] DoneReason { get; set; }
    }
}