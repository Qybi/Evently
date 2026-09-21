using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.Infrastructure.Identity;

// delegating handler is the same as a middleware but for outgoing http requests, while middleware is for incoming requests.
// It is used to add the authorization header to the outgoing http requests to the keycloak server
internal sealed class KeyCloakAuthDelegatingHandler(IOptions<KeyCloakOptions> options) : DelegatingHandler
{
    private readonly KeyCloakOptions _options = options.Value;

    /// <summary>
    /// This DelegatingHandler is used to authenticate the backend when a user is registered from the Evently API request.
    /// User registration flow
    /// 1. The application begins registering the user.
    /// 2. IdentityProviderService.RegisterUserAsync asks KeyCloakClient to create that user in Keycloak.
    /// 3. KeyCloakClient prepares POST /admin/realms/{realm}/users.
    /// 4. Before that request leaves Evently, KeyCloakAuthDelegatingHandler.SendAsync runs, it retrieves the access_token which gets injected in the original call as the Authorization Bearer token.
    /// </summary>
    /// <param name="request">The user registration request</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AuthToken authorizationToken = await GetAuthorizationToken(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken.AccessToken);

        HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();

        return httpResponseMessage;
    }

    // following keycloak documentation for confidential client authentication (safe-env machine-to-machine authentication)
    private async Task<AuthToken> GetAuthorizationToken(CancellationToken cancellationToken)
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _options.ConfidentialClientId),
            new("client_secret", _options.ConfidentialClientSecret),
            new("scope", "openid"),
            new("grant_type", "client_credentials")
        };

        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);

        using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_options.TokenUrl));

        authRequest.Content = authRequestContent;

        using HttpResponseMessage authorizationResponse = await base.SendAsync(authRequest, cancellationToken);

        return await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken);
    }

    internal sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }
}
