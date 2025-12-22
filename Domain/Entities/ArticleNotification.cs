using System;
using Domain.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ArticleNotification:Base
    {
        public int userId { get; set; }

        public int articleId { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; }= false;

        public User? _user { get; set; }

        public Article? _article { get; set; }
    }
}
