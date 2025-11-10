using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category:Base
    {
        public Category() 
        {
            _articles = new HashSet<Article>();
        
        }
        public string CategoryName { get; set; }

        public string Description { get; set; }

        // nav 

        public ICollection<Article> ?_articles { get; set; }



    }
}
