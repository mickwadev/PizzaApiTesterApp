using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaApiTesterApp;

public class AuthHeaderHandler(TokenStorage tokenStorage, IHttpClientFactory factory) : System.Net.Http.DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await base.SendAsync(request, cancellationToken);
    }
}
