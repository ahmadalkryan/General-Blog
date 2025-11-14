using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Dto.Summary
{
    public class SummaryDto
    {
        public int ID { get; set; }
        public string Summary { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int articleId { get; set; }
    }
}
