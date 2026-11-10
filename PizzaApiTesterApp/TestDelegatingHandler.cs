using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaApiTesterApp;

internal class TestDelegatingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return await Zabawy1(request, cancellationToken);
    }

    // Wysyłanie tego samego kilka razy w tym przypadku działa, ale ogólnie może się wywalić i lepiej sklonować request
    // żądania bez body, np GET można wysłać wielokrotnie, ale jak będzie post i jakiś stream, to to się może wywalić wtedy
    private async Task<HttpResponseMessage> Zabawy1(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        UriBuilder uriBuilder = new UriBuilder(request.RequestUri!);
        uriBuilder.Path = uriBuilder.Path.Replace("panmors", "ziemniaczek");
        request.RequestUri = uriBuilder.Uri;
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
        HttpResponseMessage response2 = await base.SendAsync(request, cancellationToken);
        response.EnsureSuccessStatusCode();
        response2.EnsureSuccessStatusCode();

        // return await base.SendAsync(request, cancellationToken);
        var response3 = await base.SendAsync(request, cancellationToken);
        response3.Content = new StringContent("Ale numer...");
        return response3;

    }
}
