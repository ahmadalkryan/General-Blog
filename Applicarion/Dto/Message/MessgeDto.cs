using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Message
{
    public class MessgeDto
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int senderId { get; set; }

       public string UserName { get; set; }

        public DateTime SentAt { get; set; }

        public bool IsDeleted { get; set; }


        public MessageType MessageType { get; set; }



    }
}
