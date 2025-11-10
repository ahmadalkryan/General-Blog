using Applicarion.IService;

namespace Blogs_Applications.TokenBlackListMiddleWare
{
    public class TokenBlackListMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenBlackListMiddleWare> _logger;

        public TokenBlackListMiddleWare(RequestDelegate requestDelegate ,ILogger<TokenBlackListMiddleWare> logger)
        {
            _logger = logger;
            _next = requestDelegate;

        }

        public async Task InvokeAsync(HttpContext context, ITokenBlackList _tokenBlackList)
        {
            var token = context.Request.Headers["Authorization"]
                .FirstOrDefault()?.Split(" ").Last();

            if(!string.IsNullOrEmpty(token) )
            {
                if(await _tokenBlackList.IsBlacklistedAsync(token))
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        error = "token_revoked",
                        error_description = "Token has been revoked. Please login again."
                    };

                    await context.Response.WriteAsJsonAsync(errorResponse);
                    return;
                }


            }

            await _next.Invoke(context);










        }


    }
}
