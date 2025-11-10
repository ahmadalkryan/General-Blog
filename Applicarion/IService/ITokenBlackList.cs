using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Applicarion.IService
{
    public interface ITokenBlackList
    {
       

        Task<bool> IsBlacklistedAsync(string token);
        Task BlacklistAsync(string token, DateTime expiry);
        //Task BlacklistAsync(string token, TimeSpan duration);



    }
}
