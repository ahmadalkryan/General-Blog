using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Message
{
    public class CrMessageDto
    {
        public string Content { get; set; }


       

        public MessageType MessageType { get; set; }
    }
}
