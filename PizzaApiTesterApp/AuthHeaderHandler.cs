using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace PizzaApiTesterApp;

public class AuthHeaderHandler(TokenStorage tokenStorage, IHttpClientFactory factory) : System.Net.Http.DelegatingHandler
{
    // So this is triggered each time I connect with api
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // get authentication token from storage.
        // if it is stored, add it to header
        Tokens tokens = await tokenStorage.GetTokens();
        if (tokens.jwt != null) 
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.jwt);
        }
        return await base.SendAsync(request, cancellationToken);
    }
}
