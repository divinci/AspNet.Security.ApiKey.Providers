using AspNet.Security.ApiKey.Providers.Events;
using Microsoft.AspNetCore.Authentication;

namespace AspNet.Security.ApiKey.Providers
{
    /// <summary>
    /// Contains the options used by the <see cref="ApiKeyHandler" />. 
    /// </summary>
    public class ApiKeyOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// The header that shall contain the authentication data. Defaults to "Authorization". 
        /// <para> An example header using the <see cref="ApiKeyOptions" /> defaults would be: </para>
        /// <para> Authorization: ApiKey 4fb4e33c83e5d026e8745102b72f10590f48e94af107db15074c799589a4753d </para>
        /// </summary>
        public string Header { get; set; } = "Authorization";

        /// <summary>
        /// The key of the key/value pair that represents the authentication type and its data.
        /// Defaults to "ApiKey".
        /// <para> An example header using the <see cref="ApiKeyOptions" /> defaults would be: </para>
        /// <para> Authorization: ApiKey 4fb4e33c83e5d026e8745102b72f10590f48e94af107db15074c799589a4753d </para>
        /// </summary>
        public string HeaderKey { get; set; } = "ApiKey";

        /// <summary>
        /// The object provided by the application to process events raised by the bearer
        /// authentication handler. The application may implement the interface fully, or it may
        /// create an instance of JwtBearerEvents and assign delegates only to the events it wants to process.
        /// </summary>
        public new ApiKeyEvents Events
        {
            get => (ApiKeyEvents)base.Events;
            set => base.Events = value;
        }

        public ApiKeyOptions()
        {
            this.Events = new ApiKeyEvents();
        }
    }
}
