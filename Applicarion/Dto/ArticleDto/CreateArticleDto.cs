using Applicarion.Dto.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Dto.ArticleDto
{
    public class CreateArticleDto
    {
        
        public string Title { get; set; }
        public string Content { get; set; }

        public IFormFile? Image { get; set; }

        

        public int categoryId { get; set; }

        public int userID { get; set; }

        public bool? IsPublished { get; set; } = true;



    }
}
