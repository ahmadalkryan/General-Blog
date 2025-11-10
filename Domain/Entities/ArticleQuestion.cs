using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ArticleQuestion:Base
    {
        public string Question { get; set; }

        public string Ansewr {  get; set; }

        public DateTime CreatedAt  { get; set; }= DateTime.Now;

        public  DateTime AnsweredAt {  get; set; }


        public int userId { get; set; }

        public User ?_user { get; set; }

        public int articleId { get; set; }

        public Article ?_article { get; set; }




    }
}
