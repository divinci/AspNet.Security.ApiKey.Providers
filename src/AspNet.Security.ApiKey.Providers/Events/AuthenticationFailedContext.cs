using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.ApiKey.Providers.Events
{
    public class AuthenticationFailedContext : ResultContext<ApiKeyOptions>
    {
        public AuthenticationFailedContext(HttpContext context, AuthenticationScheme scheme, ApiKeyOptions options)
            : base(context, scheme, options) { }

        public Exception Exception { get; set; }
    }
}
