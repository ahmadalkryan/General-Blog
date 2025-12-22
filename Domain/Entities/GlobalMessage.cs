using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{ 
    public class GlobalMessage:Base
    {
        public string Content { get; set; }

        public int senderId { get; set; }

        public User _user { get; set; }

        public DateTime SentAt { get; set; }= DateTime.Now;

        public bool IsDeleted { get; set; }
       

        public MessageType MessageType { get; set; }

        public ICollection<Notification> _notifications { get; set; }= new List<Notification>();


    }
}
