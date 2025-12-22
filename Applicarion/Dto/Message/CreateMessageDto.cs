using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Message
{
    public class CreateMessageDto
    {

        public string Content { get; set; }


        public int senderId { get; set; }



        public MessageType MessageType { get; set; }

    }
}
