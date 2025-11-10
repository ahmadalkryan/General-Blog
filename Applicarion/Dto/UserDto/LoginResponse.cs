using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.Dto.UserDto
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public UserDto User { get; set; }
    }
}
