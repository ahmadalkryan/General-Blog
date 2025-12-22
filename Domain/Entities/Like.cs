using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Like:Base
    {
        public int articleId { set; get; }

        public int userId { set; get; }

        public Article? _article { set; get; }

        public User? _user { set; get; }

        public DateTime CreatedAt { set; get; }= DateTime.Now;

    }
}
