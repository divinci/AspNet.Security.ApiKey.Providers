using System;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.ApiKey.Providers.Events
{
    public class ApiKeyChallengeContext : PropertiesContext<ApiKeyOptions>
    {
        public ApiKeyChallengeContext(HttpContext context, AuthenticationScheme scheme, ApiKeyOptions options, AuthenticationProperties properties)
            : base(context, scheme, options, properties) { }

        public string ApiKey { get; set; }

        /// <summary>
        /// Any failures encountered during the authentication process. 
        /// </summary>
        public Exception AuthenticateFailure { get; set; }

        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.Unauthorized;

        /// <summary>
        /// If true, will skip any default logic for this challenge. 
        /// </summary>
        public bool Handled { get; private set; }

        /// <summary>
        /// Skips any default logic for this challenge. 
        /// </summary>
        public void HandleResponse() => this.Handled = true;
    }
}
