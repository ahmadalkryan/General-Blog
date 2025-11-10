using Applicarion.IService;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service.JwtService
{
    public class TokenBlackList : ITokenBlackList
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<TokenBlackList> _logger;
        private  const string BLACKLIST_PREFIX = "blacklist_";
        private readonly MemoryCacheEntryOptions _defaultCacheOptions;

        public TokenBlackList(IMemoryCache memoryCache ,ILogger<TokenBlackList> logger)
        {
            _logger= logger;    
            _memoryCache = memoryCache;

            _defaultCacheOptions = new MemoryCacheEntryOptions
            {
                Priority = CacheItemPriority.High,
            };
        }



        public Task BlacklistAsync(string token, DateTime expiry)
        {
            var duration = expiry - DateTime.UtcNow;

            if(duration>TimeSpan.Zero)
            {
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = expiry,
                    Priority = CacheItemPriority.High,
                };
                _memoryCache.Set(token, options);
            }


            return Task.CompletedTask;
        }

        //public Task BlacklistAsync(string token, TimeSpan duration)
        //{
            
        //}

        public Task<bool> IsBlacklistedAsync(string token)
        {
            var isBlacklisted = _memoryCache.TryGetValue(token  ,out _);
            if (isBlacklisted)
            {
                _logger.LogDebug("Token found in black list ", token);

            }
            return Task.FromResult(isBlacklisted);

        }
    }
}
