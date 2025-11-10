using Applicarion.IService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class OpenAIService:IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(ILogger<OpenAIService> logger ,
            IConfiguration configuration ,HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient= httpClient;
            _logger= logger;


            var apiKey = _configuration["OpenAI:ApiKey"];
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http
                
                .Headers.AuthenticationHeaderValue("Bearer", apiKey);


        }
        public string ServiceName => "OprnAI";

        public async Task<string> GenerateAnswerAsync(string context, string question)
        {
            var apiUrl = "https://api.openai.com/v1/chat/completions";
            var messages = new[]
         {
            new
            {
                role = "system",
                content = @"أنت مساعد ذكي لمدونة عربية. قم بالإجابة على أسئلة القراء بناءً على المحتوى المقدم فقط.

                        التعليمات:
                        - أجب بناءً على المحتوى المقدم فقط
                        - إذا لم يكن المحتوى كافياً، قل 'لا أستطيع الإجابة بناءً على المحتوى المتاح'
                        - استخدم لغة عربية واضحة وسليمة
                        - كن مفيداً ودقيقاً
                        - لا تختلق معلومات"
            },
            new
            {
                role = "user",
                content = $"المحتوى المرجعي:\n{context}\n\nالسؤال: {question}"
            }
        };


            var request = new
            {
                model = _configuration["OpenAI:Model"] ?? "gpt-3.5-turbo",
                messages = messages,
                max_tokens = 1000,
                temperature = 0.3
            };



            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                    return CleanResponse(result.choices[0].message.content);
                }

                _logger.LogWarning("OpenAI API returned: {StatusCode}", response.StatusCode);
                return "عذراً، حدث خطأ في معالجة سؤالك. يرجى المحاولة لاحقاً.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OpenAI API");
                return "عذراً، تعذر الوصول إلى خدمة الذكاء الاصطناعي حالياً.";
            }

        }
        private string CleanResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
                return response;

            var cleanResponse = response.Trim();

            // البادئات التي يتم إزالتها
            var prefixes = new[] {
        "ملخص:", "الملخص:",
        "إجابة:", "الإجابة:",
        "**ملخص:**", "**إجابة:**",
        "ملخص النص:", "تلخيص النص:",
        "بناءً على النص:", "وفقًا للنص:"
    };

            // إزالة البادئات
            foreach (var prefix in prefixes)
            {
                if (cleanResponse.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                {
                    cleanResponse = cleanResponse.Substring(prefix.Length).Trim();
                    break;
                }
            }

            // تنظيف إضافي للرموز
            cleanResponse = cleanResponse.TrimStart(':', '-', '*', ' ', '\n', '\r');

            return cleanResponse;
        }
        public async  Task<string> SummarizeTextAsync(string text)
        {
            var apiUrl = "https://api.openai.com/v1/chat/completions";
            var messages = new[]
       {
            new
            {
                role = "system",
                content = @"قم بتلخيص النص في نقاط رئيسية واضحة باللغة العربية.

                    المتطلبات:
                    - اكتب الملخص باللغة العربية الفصحى
                    - ركز على الأفكار الرئيسية
                    - استخدم صياغة واضحة وسهلة الفهم
                    - لا تزيد عن 4-5 نقاط رئيسية
                    - لا تضيف معلومات خارج النص"
            },
            new
            {
                role = "user",
                content = $"النص:\n{text}"
            }
        };
            var request = new
            {
                model = _configuration["OpenAI:Model"] ?? "gpt-3.5-turbo",
                messages = messages,
                max_tokens = 800,
                temperature = 0.3
            };
            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                    return CleanResponse(result.choices[0].message.content);
                }

                return GenerateFallbackSummary(text);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating summary with OpenAI");
                return GenerateFallbackSummary(text);
            }

        }
        private string GenerateFallbackSummary(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "لا يوجد محتوى لتلخيصه.";

            try
            {
                // تقسيم النص إلى فقرات أولاً
                var paragraphs = text.Split('\n')
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Take(2)
                    .ToArray();

                if (paragraphs.Any())
                {
                    return $"ملخص النص:\n{string.Join("\n\n", paragraphs)}";
                }

                // إذا لم توجد فقرات، استخدام الجمل
                var sentences = text.Split(new[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Take(3)
                    .Select(s => s.Trim())
                    .ToArray();

                return sentences.Any() ?
                    $"النقاط الرئيسية:\n• {string.Join("\n• ", sentences)}." :
                    "تعذر توليد ملخص للمحتوى المقدم.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in fallback summary generation");
                return "عذراً، تعذر توليد ملخص للمحتوى.";
            }
        }
        public async Task<bool> IsServiceAvailableAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.openai.com/v1/models");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking OpenAI service availability.");
                return false;
            }

        }

         



    }
}
public class OpenAIResponse
{
    public Choice[] choices { get; set; }
}

public class Choice
{
    public Message message { get; set; }
}

public class Message
{
    public string content { get; set; }
}
