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

        public ICollection<GlobalMessage> _Messages { get; set; }
        = new List<GlobalMessage>();

        public ICollection<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
       
        public ICollection<ArticleApproval> _articleApprovals { get; set; }= new List<ArticleApproval>();

        public ICollection<ArticleNotification> _articleNotifications { get; set; }= new List<ArticleNotification>();   
        public Persona _userProfile { get; set; }

        public Like _like { get; set; }


    }

}
