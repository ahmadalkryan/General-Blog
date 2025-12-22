using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Notification:Base
    {
        public string Title { get; set; }
        public string MessageNotification { get; set; }

        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; } =false;
      

        
        public int receiverId { get; set; }


        public int MessageId { get; set; }

        

        public User? _receiver { get; set; }
        public  GlobalMessage? _globalMessage { get; set; }
    }
}
