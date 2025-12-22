using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Profile
{
    public class CreateProfile
    {
        public int userId { get; set; }

        public string UserName { get; set; }

        public string Bio { get; set; }

        public string Location { get; set; }


        public string PhoneNumber { get; set; }


    }
}
