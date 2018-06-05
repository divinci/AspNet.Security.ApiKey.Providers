using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AspNet.Security.ApiKey.Providers.Abstractions;
using AspNet.Security.ApiKey.Providers.Events;
using AspNet.Security.ApiKey.Providers.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AspNet.Security.ApiKey.Providers
{
    public class ApiKeyHandler : AuthenticationHandler<ApiKeyOptions, ApiKeyEvents>
    {
        public ApiKeyHandler(IOptionsMonitor<ApiKeyOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                var messageReceivedContext = new MessageReceivedContext(this.Context, this.Scheme, this.Options);

                await this.Events.MessageReceived(messageReceivedContext);

                string apiKey = messageReceivedContext.ApiKey;

                if (string.IsNullOrEmpty(apiKey))
                {
                    string header = this.Request.Headers[this.Options.Header];

                    if (string.IsNullOrEmpty(header))
                    {
                        this.Logger.ApiKeyValidationFailed();

                        return AuthenticateResult.NoResult();
                    }

                    if (header.StartsWith(this.Options.HeaderKey, StringComparison.OrdinalIgnoreCase))
                    {
                        apiKey = header.Substring(this.Options.HeaderKey.Length).Trim();
                    }
                }

                var validateApiKeyContext = new ApiKeyValidatedContext(this.Context, this.Scheme, this.Options)
                {
                    ApiKey = apiKey
                };

                await this.Events.ApiKeyValidated(validateApiKeyContext);

                if (validateApiKeyContext.Result != null)
                {
                    this.Logger.ApiKeyValidationSucceeded();

                    return validateApiKeyContext.Result;
                }

                this.Logger.ApiKeyValidationFailed();

                return AuthenticateResult.NoResult();
            }
            catch (Exception ex)
            {
                this.Logger.ErrorProcessingMessage(ex);

                var authenticationFailedContext = new AuthenticationFailedContext(this.Context, this.Scheme, this.Options)
                {
                    Exception = ex
                };

                await this.Events.AuthenticationFailed(authenticationFailedContext);

                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                throw;
            }
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            var authResult = await this.HandleAuthenticateOnceSafeAsync();

            if (authResult.Succeeded)
            {
                return;
            }

            var eventContext = new ApiKeyChallengeContext(this.Context, this.Scheme, this.Options, properties)
            {
                AuthenticateFailure = authResult.Failure
            };

            await this.Events.Challenge(eventContext);

            if (eventContext.Handled)
            {
                return;
            }

            this.Response.StatusCode = (int)eventContext.StatusCode;
        }
    }
}
