using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User:Base
    {
        public User() { 
        
        _articles = new List<Article>();
            _comments = new List<Comment>();
            _articleQuestions= new List<ArticleQuestion>();
        }
        public string UserName { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; } // Admin , Writer , User

        public ICollection<ArticleQuestion>? _articleQuestions { get; set; }
        public ICollection<Article> ?_articles { get; set; }
        public ICollection<Comment> ?_comments { get; set; }


    }

}
