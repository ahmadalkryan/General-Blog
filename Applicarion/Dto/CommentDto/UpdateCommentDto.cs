using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Dto.CommentDto
{
    public class UpdateCommentDto
    {
        public int ID { get; set; }
        public string Content { get; set; }



        public int articleID { get; set; }
    }
}
