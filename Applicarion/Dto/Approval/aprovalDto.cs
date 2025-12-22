using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Approval
{
    public class aprovalDto
    {
        public int ID { get; set; }
        public int _articleId { get; set; }

        public int _adminId { get; set; }

        public string? RejectReason { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        public ApprovalStatus Status { get; set; } = ApprovalStatus.Pending; ////  Pending,  Approved, Rejected

    }
}
