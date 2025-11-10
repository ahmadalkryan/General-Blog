using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ArticleSummary:Base
    {
        public string Summary { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        public int articleId {  get; set; }

        public Article _article { get; set; }


    }
}
