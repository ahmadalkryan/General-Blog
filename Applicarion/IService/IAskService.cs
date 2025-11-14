using Applicarion.Dto.ArticelQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.IService
{
    public interface IAskService
    {

        Task<string> AskQuestion(CreateAnswer createAnswer);
    }
}
