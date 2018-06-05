using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace AspNet.Security.ApiKey.Providers.Events
{
    public class MessageReceivedContext : BaseContext<ApiKeyOptions>
    {
        public MessageReceivedContext(HttpContext context, AuthenticationScheme scheme, ApiKeyOptions options)
            : base(context, scheme, options)
        {
        }

        /// <summary>
        /// API Key. This will give the application an opportunity to retrieve an API key from an
        /// alternative location.
        /// </summary>
        public string ApiKey { get; set; }
    }
}
