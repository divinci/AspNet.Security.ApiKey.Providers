# AspNet.Security.ApiKey.Providers
 API Key authentication middleware for ASP.NET Core.

## Getting started
Grab the package from NuGet, which will install all dependencies.

`Install-Package AspNet.Security.ApiKey.Providers`

## Usage
First add this authentication type to your pipeline:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(options =>
    {
        options.DefaultScheme = ApiKeyDefaults.AuthenticationScheme;
    })
    .AddApiKey();
}
```

And enable authentication in your app:

```csharp
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    app.UseAuthentication();
}
```

You must then wire up your custom delegates to handle validation of incoming keys, as well as things like handling of errors during the authentication process:

```csharp
options.Events = new ApiKeyEvents
{
    // Optional
    OnAuthenticationFailed = context =>
    {
        Trace.TraceError(context.Exception.Message);

        return Task.CompletedTask;
    },
    OnApiKeyValidated = context =>
    {
        if (context.ApiKey == "123")
        {
            // Build and set the context.Principal if you wish to attach an identity to your incoming request.
            context.Principal = new ClaimsPrincipal();

            // Mark success if you are happy the API key in the request is valid.
            context.Success();
        }

        return Task.CompletedTask;
    }
};
```

### Customising Header Values
The format of the expected header containing the API key is completely customisable. By default, it expects a header in the following format:

```
Authorization: ApiKey {key}
```

If you wish to override this format, override the default values when configuring your `ApiKeyOptions`. For example:

```csharp
// Authorization: MyType {key}

public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(options =>
    {
        options.DefaultScheme = ApiKeyDefaults.AuthenticationScheme;
    })
    .AddApiKey(options =>
    {
        options.Header = "Authorization";
        options.HeaderKey = "MyType";
    });
}

// X-API-KEY: {key}

public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = ApiKeyDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = ApiKeyDefaults.AuthenticationScheme;
    })
    .AddApiKey(options =>
    {
        options.Header = "X-API-KEY";
        options.HeaderKey = String.Empty;
    });
}
```

### Custom Status Codes
If you wish to challenge the authentication result, you can set up a delegate to do this. For example, if you wish to return a different status code (e.g. a client's subscription has expired) you could check the reason for failure and change the code as appropriate:

```csharp
options.Events = new ApiKeyEvents
{
    OnApiKeyValidated = context =>
    {
        if (context.ApiKey == "123")
        {
            context.Principal = new ClaimsPrincipal();

            context.Success();
        }
        else if (context.ApiKey == "789")
        {
            throw new NotSupportedException("You must upgrade.");
        }

        return Task.CompletedTask;
    },
    OnChallenge = context =>
    {
        if (context.AuthenticateFailure is NotSupportedException)
        {
            context.StatusCode = HttpStatusCode.UpgradeRequired;
        }

        return Task.CompletedTask;
    }
};
```

### Advanced Usage
You can override the parsing of headers by wiring up an `OnMessageReceived` delegate. If you set an API key here then the parsing of the headers will be skipped - this may aid with things like testing.

```csharp
options.Events = new ApiKeyEvents
{
    OnMessageReceived = context =>
    {
        context.ApiKey = "123";

        return Task.CompletedTask;
    }
};
```

You can use the ASP.NET Core `IConfigureOptions<T>` and `IPostConfigureOptions<T>` interfaces to help you set up your options as appropriate. Simply add these to your services and let ASP.NET Core take care of the rest:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IConfigureOptions<ApiKeyOptions>, MyConfigureOptions>());
    services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<ApiKeyOptions>, MyPostConfigureOptions>());
}
```
