using ApiService;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace PizzaApiTesterApp;

// This is basic version without 401 handling
public class AuthHeaderHandler(TokenStorage tokenStorage) : System.Net.Http.DelegatingHandler
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

public class AuthHeaderHandlerWith401(TokenStorage tokenStorage,
    IHttpClientFactory httpClientFactory // ← this is used to ask for new tokens, this HttpClient wont have this handler
    ):DelegatingHandler
{
    // To się wywoła zanim pójdzie request do api
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if(IsAuthEndpoint(request))
        {
            return await base.SendAsync(request, cancellationToken);
        }
        // jeśli tu jesteśmy, to znaczy, że to nie jest /login i /refresh.
        // Wysyłamy zatem ten request i sprawdzamy co przyjdzie:
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
        {
            return response;
           HttpClient refresher = httpClientFactory.CreateClient("refresher");
        }
        return response;
    }

    // po piersze oba te endpointy nie potrzebują jwt. /auth/refresh potrzebuje tego refresh tokena., login nic nie potrzebuje
    // ale ważniejsze jest to, że jeśli login lub refresh zwrócą 401
    // to oznacza złe hasło/user lub lipny refresh token, zatem też dostaniemy 401 i by się pętla zrobiła >.<
    private bool IsAuthEndpoint(HttpRequestMessage request)
    {
        var path = request.RequestUri?.AbsolutePath ?? "path missing";
        return path.EndsWith("/login") || path.EndsWith("/refresh");
    }

    // tu będzie zapytanie do api po nowe tokeny do /refresh
    // ale z użyciem HttpClient bez tego handlera.
    // co wysyłam? refresh token.
    // co dostaję? jwt oraz nowy refresh token lub 401 jeśli refresh token jest za stary
    private async Task GetNewTokens(string refreshToken)
    {
        var client = httpClientFactory.CreateClient("refresher");

        // post as json ułatwia wysyłanie,
        var response = await client.PostAsJsonAsync("/auth/refresh", new RefreshUserRequestDto() { RefreshToken = refreshToken });

    }
}
