using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Comment:Base
    {
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }= DateTime.Now;

        //Nav 

        public int articleID { get; set; }

        public Article? _article { get; set; }

        public int userID { get; set; }

        public User? _user { get; set; }

    }
}
