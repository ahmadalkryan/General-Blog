using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Notification
{
    public  class NotificationDto
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string MessageNotification { get; set; }

        public DateTime SentAt { get; set; }

        public bool IsRead { get; set; } = false;

        

        public int receiverId { get; set; }


        public int MessageId { get; set; }

    }
}
