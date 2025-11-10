using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.IService
{
    public interface IAIService
    {
        Task<string> GenerateAnswerAsync(string context, string question);
        Task<string> SummarizeTextAsync(string text);
        Task<bool> IsServiceAvailableAsync();
        string ServiceName { get; }
    }
}
