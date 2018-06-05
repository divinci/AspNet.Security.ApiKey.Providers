using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace AspNet.Security.ApiKey.Providers.Extensions
{
    /// <summary>
    /// Extension methods to add API Key authentication capabilities to an HTTP application pipeline. 
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder)
            => builder.AddApiKey(ApiKeyDefaults.AuthenticationScheme, _ => { });

        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, Action<ApiKeyOptions> configureOptions)
            => builder.AddApiKey(ApiKeyDefaults.AuthenticationScheme, configureOptions);

        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, string authenticationScheme, Action<ApiKeyOptions> configureOptions)
            => builder.AddApiKey(authenticationScheme, "API Key", configureOptions);

        public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<ApiKeyOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ApiKeyOptions>, ApiKeyPostConfigureOptions>());
            builder.AddScheme<ApiKeyOptions, ApiKeyHandler>(authenticationScheme, displayName, configureOptions);

            return builder;
        }
    }
}
