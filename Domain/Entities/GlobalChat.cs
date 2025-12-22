using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class GlobalChat:Base
    {
        public string Name { get; set; }

        public string Description { get; set; } = "global chat ";

          public DateTime CreatedAt { get; set; } = DateTime.Now;
          
     //   public ICollection<GlobalMessage> Messages { get; set; }= new List<GlobalMessage>();
    }
}
