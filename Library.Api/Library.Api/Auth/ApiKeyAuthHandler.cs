﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Library.Api.Auth;

public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthSchemeOptions>
{
    public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthSchemeOptions> options,
                             ILoggerFactory logger,
                             UrlEncoder encoder,
                             ISystemClock clock)
            : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Api Key"));
        }

        var authorization = Request.Headers[HeaderNames.Authorization].ToString();
        if (authorization != Options.ApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Api Key"));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, "some@mail.com"),
            new Claim(ClaimTypes.Name, "some name")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "ApiKey");
        var ticket = new AuthenticationTicket(new ClaimsPrincipal(claimsIdentity), Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
