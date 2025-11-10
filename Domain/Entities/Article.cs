using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public  class Article:Base
    {
        public Article() 
        { 
            _comments = new HashSet<Comment>();
            _articleQuestions= new HashSet<ArticleQuestion>();
        }
        public string Title { get; set; }
        public string Content   { get; set; }
     
        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public DateTime UpdatedAt { get; set; }=DateTime.Now;

        public bool IsPublished { get; set; }=true;



        // nav 
        public int categoryId { get; set; }
        public ICollection<Comment> ?_comments { get; set; }
        public ICollection<ArticleQuestion>? _articleQuestions { get; set; }
        public int userID { get; set; }

        public ArticleSummary? articleSummary { get; set; }
        public Category ?_category { get; set; }

        public User? _user   { get; set; }

    }
}
