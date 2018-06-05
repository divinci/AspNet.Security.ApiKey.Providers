using System;
using Microsoft.Extensions.Options;

namespace AspNet.Security.ApiKey.Providers
{
    public class ApiKeyPostConfigureOptions : IPostConfigureOptions<ApiKeyOptions>
    {
        public void PostConfigure(string name, ApiKeyOptions options)
        {
            if (String.IsNullOrWhiteSpace(options.Header))
            {
                throw new ArgumentException("Header must not be null or whitespace.", nameof(options.Header));
            }

            if (options.HeaderKey is null)
            {
                throw new ArgumentException("Header key must not be null.", nameof(options.HeaderKey));
            }
        }
    }
}
