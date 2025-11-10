using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public class Base:IEquatable<Base>
    {
        public int ID { get; set; }

        public  bool Equals(Base? other)
        {
            throw new NotImplementedException();
        }
    }
}
