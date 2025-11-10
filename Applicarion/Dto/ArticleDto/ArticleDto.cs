using Applicarion.Dto.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Dto.ArticleDto
{
    public class ArticleDto
    {
        public int ID{ get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public  string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } 

        public DateTime UpdatedAt { get; set; } 

        public bool IsPublished { get; set; } = true;


        public int categoryId { get; set; }
        public string CategoryName { get; set; }

       
        public int userID { get; set; }
        public string AuthorName { get; set; }

       

    }
}
