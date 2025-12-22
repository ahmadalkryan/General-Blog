using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.ArticleNotification
{
    public class NotiDto
    {
        public int ID { get; set; }
        public int userId { get; set; }

        public int articleId { get; set; }
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;
    }
}
