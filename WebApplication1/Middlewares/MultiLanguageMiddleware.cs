using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.Middlewares
{
    public class MultiLanguageMiddleware
    {
        private RequestDelegate _nextMiddleware;

        public MultiLanguageMiddleware(RequestDelegate next)
        {
            _nextMiddleware = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var defaultCulture = "en-US";//默认英文
            var cultureCookies = context.Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];

            if (!string.IsNullOrEmpty(cultureCookies)) defaultCulture = cultureCookies;

            context.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                defaultCulture,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            Thread.CurrentThread.CurrentCulture = new CultureInfo(defaultCulture);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(defaultCulture);

            await _nextMiddleware.Invoke(context);
        }
    }
}
