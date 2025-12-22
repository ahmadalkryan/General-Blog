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
            _likes = new HashSet<Like>();

        }
        public string Title { get; set; }
        public string Content   { get; set; }
     
        public string ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public DateTime UpdatedAt { get; set; }=DateTime.Now;

        public bool IsPublished { get; set; }=false;



        // nav 
        public int categoryId { get; set; }
        public ICollection<Comment> ?_comments { get; set; }
        public ICollection<ArticleQuestion>? _articleQuestions { get; set; }
        public ICollection<Like>? _likes { get; set; }
        public int userID { get; set; }

        public ArticleSummary? articleSummary { get; set; }
        public Category ?_category { get; set; }
        public ArticleApproval?  articleApproval { get; set; }

        //public ICollection<ArticleApproval> _articleApprovals { get; set; }= new List<ArticleApproval>();
        public ICollection<ArticleNotification> _articleNotifications { get; set; }= new List<ArticleNotification>();
        public User? _user   { get; set; }

    }
}
