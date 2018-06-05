using System;
using System.Threading.Tasks;

namespace AspNet.Security.ApiKey.Providers.Events
{
    /// <summary>
    /// Specifies events which the <see cref="ApiKeyHandler" /> invokes to enable developer control
    /// over the authentication process.
    /// </summary>
    public class ApiKeyEvents
    {
        /// <summary>
        /// Invoked if exceptions are thrown during request processing. The exceptions will be
        /// re-thrown after this event unless suppressed.
        /// </summary>
        public Func<AuthenticationFailedContext, Task> OnAuthenticationFailed { get; set; } = context => Task.CompletedTask;

        /// <summary>
        /// Invoked when a protocol message is first received. 
        /// </summary>
        public Func<MessageReceivedContext, Task> OnMessageReceived { get; set; } = context => Task.CompletedTask;

        /// <summary>
        /// Invoked after the API key has passed validation and a ClaimsIdentity has been generated. 
        /// </summary>
        public Func<ApiKeyValidatedContext, Task> OnApiKeyValidated { get; set; } = context => Task.CompletedTask;

        /// <summary>
        /// Invoked before a challenge is sent back to the caller. 
        /// </summary>
        public Func<ApiKeyChallengeContext, Task> OnChallenge { get; set; } = context => Task.CompletedTask;

        public virtual Task AuthenticationFailed(AuthenticationFailedContext context) => this.OnAuthenticationFailed(context);

        public virtual Task MessageReceived(MessageReceivedContext context) => this.OnMessageReceived(context);

        public virtual Task ApiKeyValidated(ApiKeyValidatedContext context) => this.OnApiKeyValidated(context);

        public virtual Task Challenge(ApiKeyChallengeContext context) => this.OnChallenge(context);
    }
}
