using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ArticleApproval: Base
    {
       public int _articleId { get; set; }

        public int _adminId { get; set; }

        public string? RejectReason { get; set; }
        public DateTime CreateAt { get; set; }= DateTime.Now;

        public ApprovalStatus Status { get; set; }= ApprovalStatus.Pending;

        public User User { get; set; }

        public Article Article { get; set; }








    }
}
