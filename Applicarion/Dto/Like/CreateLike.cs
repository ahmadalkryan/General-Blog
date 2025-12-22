using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Like
{
    public class CreateLike
    {
        public int articleId { set; get; }

        public int userId { set; get; }
    }
}
