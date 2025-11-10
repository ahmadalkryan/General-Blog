using Applicarion.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        private readonly BlogDbContext _db;

        public ArticleRepository(BlogDbContext blogDbContext):base(blogDbContext) 
        {
            _db = blogDbContext;
        }

        public async Task<IEnumerable<Article>> GetAllWithInclude()
        {
           

            return await _db._articles.Include(a => a._user).Include(a => a._category).ToListAsync();
        }
    }
}
