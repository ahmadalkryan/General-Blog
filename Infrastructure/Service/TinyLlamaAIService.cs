using Applicarion.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class TinyLlamaAIService : IAIService
    {
        private readonly HttpClient _httpClient;

        public string ServiceName => "TinyLlama AI Service";

        public TinyLlamaAIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://localhost:11434");
            _httpClient.Timeout = TimeSpan.FromMinutes(10);
        }

        public async Task<string> GenerateAnswerAsync(string context, string question)
        {
            try
            {
                var prompt = $@"Context: {context}

Question: {question}

Please provide a clear and concise answer in English:";

                var requestData = new
                {
                    model = "tinyllama",
                    prompt = prompt,
                    stream = false,
                    options = new
                    {
                        temperature = 0.1,  // أكثر دقة
                        top_p = 0.9,
                        num_predict = 150   // تحديد الطول
                    }
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/generate", content);

                if (!response.IsSuccessStatusCode)
                {
                    return $"API Error: {response.StatusCode}";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OllamaResponse>(responseContent);

                return CleanResponse(result?.Response) ?? "No response generated";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> SummarizeTextAsync(string text)
        {
            try
            {
                var prompt = $@"Please summarize the following text in English. 
Be concise, clear, and focus on main ideas only:

Text: {text}

Summary:";

                var requestData = new
                {
                    model = "tinyllama",
                    prompt = prompt,
                    stream = false,
                    options = new
                    {
                        temperature = 0.1,
                        top_p = 0.9,
                        num_predict = 100
                    }
                };

                var json = JsonSerializer.Serialize(requestData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/generate", content);

                if (!response.IsSuccessStatusCode)
                {
                    return $"API Error: {response.StatusCode}";
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<OllamaResponse>(responseContent);

                return CleanResponse(result?.Response) ?? "No summary generated";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        private string CleanResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
                return response;

            // إزالة الأسطر الفارغة والمسافات الزائدة
            return response.Trim()
                          .Replace("\n", " ")
                          .Replace("  ", " ");
        }

        public async Task<bool> IsServiceAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/tags");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    public class OllamaResponse
    {
        public string Model { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Response { get; set; }
        public bool Done { get; set; }
    }
}

