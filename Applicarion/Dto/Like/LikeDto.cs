using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Like
{
    public  class LikeDto
    {
        public int ID { set; get; }
        public int articleId { set; get; }
        public int userId { set; get; }

        public string Username { set; get; }
        public DateTime CreatedAt { set; get; } = DateTime.Now;
    }
}
