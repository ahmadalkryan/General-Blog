using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IService
{
    public interface IAIService
    {
        Task<string> GenerateAnswerAsync(string context, string question);
        Task<string> SummarizeTextAsync(string text);
        Task<bool> IsServiceAvailableAsync();

          Task<string> GetModelInfoAsync();
        string ServiceName { get; }
    }
}

