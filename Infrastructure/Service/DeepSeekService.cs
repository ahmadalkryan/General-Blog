////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;

////namespace Infrastructure.Service
////{

////}
//using Applicarion.IService;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace Infrastructure.Service
//{
//    public class DeepSeekService : IAIService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly IConfiguration _configuration;
//        private readonly ILogger<DeepSeekService> _logger;

//        public DeepSeekService(
//            ILogger<DeepSeekService> logger,
//            IConfiguration configuration,
//            HttpClient httpClient)
//        {
//            _configuration = configuration;
//            _httpClient = httpClient;
//            _logger = logger;

//            // إعداد API Key لـ DeepSeek
//            var apiKey = _configuration["DeepSeek:ApiKey"] ?? "sk-9b01d4933ded4567ac27412951ee600c";
//            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
//        }

//        public string ServiceName => "DeepSeek";

//        public async Task<string> GenerateAnswerAsync(string context, string question)
//        {
//            var apiUrl = "https://api.deepseek.com/v1/chat/completions";

//            var messages = new[]
//            {
//                new
//                {
//                    role = "system",
//                    content = @"أنت مساعد ذكي. قم بالإجابة على الأسئلة بناءً على المحتوى المقدم فقط.
//                              - أجب بناءً على المحتوى المقدم فقط
//                              - إذا لم يكن المحتوى كافياً، قل 'لا أستطيع الإجابة بناءً على المحتوى المتاح'
//                              - استخدم لغة عربية واضحة وسليمة
//                              - كن مفيداً ودقيقاً
//                              - لا تختلق معلومات"
//                },
//                new
//                {
//                    role = "user",
//                    content = $"المحتوى المرجعي:\n{context}\n\nالسؤال: {question}"
//                }
//            };

//            var request = new
//            {
//                model = "deepseek-chat",
//                messages = messages,
//                max_tokens = 1000,
//                temperature = 0.3
//            };

//            try
//            {
//                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<DeepSeekResponse>();
//                    return CleanResponse(result.choices[0].message.content);
//                }

//                _logger.LogWarning("DeepSeek API returned: {StatusCode}", response.StatusCode);
//                return "عذراً، حدث خطأ في معالجة سؤالك. يرجى المحاولة لاحقاً.";
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error calling DeepSeek API");
//                return "عذراً، تعذر الوصول إلى خدمة الذكاء الاصطناعي حالياً.";
//            }
//        }

//        public async Task<string> SummarizeTextAsync(string text)
//        {
//            var apiUrl = "https://api.deepseek.com/v1/chat/completions";

//            var messages = new[]
//            {
//                new
//                {
//                    role = "system",
//                    content = @"قم بتلخيص النص في نقاط رئيسية واضحة باللغة العربية.
//                            - ركز على الأفكار الرئيسية
//                            - استخدم صياغة واضحة وسهلة الفهم
//                            - لا تزيد عن 4-5 نقاط رئيسية
//                            - لا تضيف معلومات خارج النص"
//                },
//                new
//                {
//                    role = "user",
//                    content = $"النص:\n{text}"
//                }
//            };

//            var request = new
//            {
//                model = "deepseek-chat",
//                messages = messages,
//                max_tokens = 800,
//                temperature = 0.3
//            };

//            try
//            {
//                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<DeepSeekResponse>();
//                    return CleanResponse(result.choices[0].message.content);
//                }

//                return GenerateFallbackSummary(text);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error generating summary with DeepSeek");
//                return GenerateFallbackSummary(text);
//            }
//        }

//        public async Task<bool> IsServiceAvailableAsync()
//        {
//            try
//            {
//                // اختبار بسيط للخدمة
//                var testRequest = new
//                {
//                    model = "deepseek-chat",
//                    messages = new[]
//                    {
//                        new { role = "user", content = "Say 'hello'" }
//                    },
//                    max_tokens = 5
//                };

//                var response = await _httpClient.PostAsJsonAsync(
//                    "https://api.deepseek.com/v1/chat/completions",
//                    testRequest
//                );

//                return response.IsSuccessStatusCode;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error checking DeepSeek service availability.");
//                return false;
//            }
//        }

//        private string CleanResponse(string response)
//        {
//            if (string.IsNullOrEmpty(response))
//                return response;

//            var cleanResponse = response.Trim();

//            // إزالة البادئات الشائعة
//            var prefixes = new[] {
//                "ملخص:", "الملخص:",
//                "إجابة:", "الإجابة:",
//                "**ملخص:**", "**إجابة:**"
//            };

//            foreach (var prefix in prefixes)
//            {
//                if (cleanResponse.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
//                {
//                    cleanResponse = cleanResponse.Substring(prefix.Length).Trim();
//                    break;
//                }
//            }

//            return cleanResponse.TrimStart(':', '-', '*', ' ', '\n', '\r');
//        }

//        private string GenerateFallbackSummary(string text)
//        {
//            if (string.IsNullOrWhiteSpace(text))
//                return "لا يوجد محتوى لتلخيصه.";

//            try
//            {
//                var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
//                    .Where(s => !string.IsNullOrWhiteSpace(s))
//                    .Take(3)
//                    .Select(s => s.Trim())
//                    .ToArray();

//                return sentences.Any() ?
//                    $"النقاط الرئيسية:\n• {string.Join("\n• ", sentences)}." :
//                    "تعذر توليد ملخص للمحتوى المقدم.";
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in fallback summary generation");
//                return "عذراً، تعذر توليد ملخص للمحتوى.";
//            }
//        }
//    }

//    public class DeepSeekResponse
//    {
//        public Choice[] choices { get; set; }
//    }

//    public class Choice
//    {
//        public Message message { get; set; }
//    }

//    public class Message
//    {
//        public string role { get; set; }
//        public string content { get; set; }
//    }
//}

//using Applicarion.IService;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;

//namespace Infrastructure.Service
//{
//    public class DeepSeekService : IAIService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly IConfiguration _configuration;
//        private readonly ILogger<DeepSeekService> _logger;

//        public DeepSeekService(
//            ILogger<DeepSeekService> logger,
//            IConfiguration configuration,
//            HttpClient httpClient)
//        {
//            _configuration = configuration;
//            _httpClient = httpClient;
//            _logger = logger;

//            var apiKey = _configuration["DeepSeek:ApiKey"] ?? "sk-9b01d4933ded4567ac27412951ee600c";
//            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
//        }

//        public string ServiceName => "DeepSeek";

//        public async Task<string> GenerateAnswerAsync(string context, string question)
//        {
//            var apiUrl = "https://api.deepseek.com/v1/chat/completions";

//            var messages = new[]
//            {
//                new
//                {
//                    role = "system",
//                    content = @"You are a smart assistant for an English blog. Answer readers' questions based only on the provided content.

//Instructions:
//- Answer based only on the provided content
//- If the content is insufficient, say 'I cannot answer based on the available content'
//- Use clear and proper English
//- Be helpful and accurate
//- Do not make up information"
//                },
//                new
//                {
//                    role = "user",
//                    content = $"Reference Content:\n{context}\n\nQuestion: {question}"
//                }
//            };

//            var request = new
//            {
//                model = "deepseek-chat",
//                messages = messages,
//                max_tokens = 1000,
//                temperature = 0.3
//            };

//            try
//            {
//                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<DeepSeekResponse>();
//                    return CleanResponse(result.choices[0].message.content);
//                }

//                _logger.LogWarning("DeepSeek API returned: {StatusCode}", response.StatusCode);
//                return "Sorry, there was an error processing your question. Please try again later.";
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error calling DeepSeek API");
//                return "Sorry, the AI service is currently unavailable.";
//            }
//        }

//        public async Task<string> SummarizeTextAsync(string text)
//        {
//            var apiUrl = "https://api.deepseek.com/v1/chat/completions";

//            var messages = new[]
//            {
//                new
//                {
//                    role = "system",
//                    content = @"Summarize the text in clear main points in English.

//                                    Requirements:
//                                    - Focus on main ideas
//                                    - Use clear and easy-to-understand wording
//                                    - Maximum 4-5 main points
//                                    - Do not add information outside the text"
//                },
//                new
//                {
//                    role = "user",
//                    content = $"Text:\n{text}"
//                }
//            };

//            var request = new
//            {
//                model = "deepseek-chat",
//                messages = messages,
//                max_tokens = 800,
//                temperature = 0.3
//            };

//            try
//            {
//                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

//                if (response.IsSuccessStatusCode)
//                {
//                    var result = await response.Content.ReadFromJsonAsync<DeepSeekResponse>();
//                    return CleanResponse(result.choices[0].message.content);
//                }

//                return GenerateFallbackSummary(text);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error generating summary with DeepSeek");
//                return GenerateFallbackSummary(text);
//            }
//        }

//        public async Task<bool> IsServiceAvailableAsync()
//        {
//            try
//            {
//                var testRequest = new
//                {
//                    model = "deepseek-chat",
//                    messages = new[]
//                    {
//                        new { role = "user", content = "Say 'hello'" }
//                    },
//                    max_tokens = 5
//                };

//                var response = await _httpClient.PostAsJsonAsync(
//                    "https://api.deepseek.com/v1/chat/completions",
//                    testRequest
//                );

//                return response.IsSuccessStatusCode;
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error checking DeepSeek service availability.");
//                return false;
//            }
//        }

//        private string CleanResponse(string response)
//        {
//            if (string.IsNullOrEmpty(response))
//                return response;

//            var cleanResponse = response.Trim();

//            var prefixes = new[] {
//                "Summary:", "The summary:",
//                "Answer:", "The answer:",
//                "**Summary:**", "**Answer:**"
//            };

//            foreach (var prefix in prefixes)
//            {
//                if (cleanResponse.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
//                {
//                    cleanResponse = cleanResponse.Substring(prefix.Length).Trim();
//                    break;
//                }
//            }

//            return cleanResponse.TrimStart(':', '-', '*', ' ', '\n', '\r');
//        }

//        private string GenerateFallbackSummary(string text)
//        {
//            if (string.IsNullOrWhiteSpace(text))
//                return "No content to summarize.";

//            try
//            {
//                var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
//                    .Where(s => !string.IsNullOrWhiteSpace(s))
//                    .Take(3)
//                    .Select(s => s.Trim())
//                    .ToArray();

//                return sentences.Any() ?
//                    $"Main points:\n• {string.Join("\n• ", sentences)}." :
//                    "Could not generate summary for the provided content.";
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error in fallback summary generation");
//                return "Sorry, could not generate summary for the content.";
//            }
//        }
//    }

//    public class DeepSeekResponse
//    {
//        public Choice[] choices { get; set; }
//    }

//    public class Choice
//    {
//        public Message message { get; set; }
//    }

//    public class Message
//    {
//        public string role { get; set; }
//        public string content { get; set; }
//    }
//}


using Applicarion.IService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

public class AIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AIService> _logger;

    public AIService(
        ILogger<AIService> logger,
        IConfiguration configuration,
        HttpClient httpClient)
    {
        _configuration = configuration;
        _httpClient = httpClient;
        _logger = logger;
    }

    public string ServiceName => "HuggingFace";

    public async Task<string> GenerateAnswerAsync(string context, string question)
    {
        var apiUrl = "https://api-inference.huggingface.co/models/microsoft/DialoGPT-large";

        var request = new
        {
            inputs = $"{context}\n\nQuestion: {question}",
            parameters = new { max_length = 500 }
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HuggingFaceResponse[]>();
                return CleanResponse(result[0].generated_text);
            }

            _logger.LogWarning("Hugging Face API returned: {StatusCode}", response.StatusCode);
            return "Sorry, there was an error processing your question. Please try again later.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Hugging Face API");
            return "Sorry, the AI service is currently unavailable.";
        }
    }

    public async Task<string> SummarizeTextAsync(string text)
    {
        var apiUrl = "https://api-inference.huggingface.co/models/facebook/bart-large-cnn";

        var request = new
        {
            inputs = text,
            parameters = new { max_length = 150, min_length = 30, do_sample = false }
        };

        try
        {
            var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<HuggingFaceSummary[]>();
                return CleanResponse(result[0].summary_text);
            }

            // إذا كان النموذج يحمّل، انتظر وحاول مرة أخرى
            if (response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            {
                await Task.Delay(5000);
                response = await _httpClient.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<HuggingFaceSummary[]>();
                    return CleanResponse(result[0].summary_text);
                }
            }

            return GenerateFallbackSummary(text);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating summary with Hugging Face");
            return GenerateFallbackSummary(text);
        }
    }

    public async Task<bool> IsServiceAvailableAsync()
    {
        try
        {
            var testRequest = new { inputs = "Test" };
            var response = await _httpClient.PostAsJsonAsync(
                "https://api-inference.huggingface.co/models/facebook/bart-large-cnn",
                testRequest
            );

            return response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking Hugging Face service availability.");
            return false;
        }
    }

    private string CleanResponse(string response)
    {
        if (string.IsNullOrEmpty(response))
            return response;

        return response.Trim();
    }

    private string GenerateFallbackSummary(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "No content to summarize.";

        try
        {
            var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Take(3)
                .Select(s => s.Trim() + ".")
                .ToArray();

            return sentences.Any() ?
                string.Join(" ", sentences) :
                text.Length > 150 ? text.Substring(0, 150) + "..." : text;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in fallback summary generation");
            return "Summary: " + (text.Length > 100 ? text.Substring(0, 100) + "..." : text);
        }
    }
}

public class HuggingFaceSummary
{
    public string summary_text { get; set; }
}

public class HuggingFaceResponse
{
    public string generated_text { get; set; }
}