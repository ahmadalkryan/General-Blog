using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.IRepository
{
    public interface IArticleRepository:IRepository<Article>
    {
        Task<IEnumerable<Article>> GetAllWithInclude();
    }
}
