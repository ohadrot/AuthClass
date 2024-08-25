using AuthClass.Context;
using AuthClass.Model;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthClass.Middlewares
{
    public class CookieMiddleWare
    {
        private readonly RequestDelegate _next;
        public CookieMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context,UserContext _dbContext)
        {
            if (context.Request.Path.StartsWithSegments("/home"))
            {
                if (context.Request.Cookies.TryGetValue("Auth", out var token))
                {
                        User user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Token == token);

                        if (user != null && token != null && token == user.Token )
                        {
                            await _next.Invoke(context);
                            return;
                        }
                }
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("access denied");
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}
