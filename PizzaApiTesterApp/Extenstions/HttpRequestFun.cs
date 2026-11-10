using CommunityToolkit.Maui;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaApiTesterApp.Extenstions;

internal static class HttpRequestFun
{
    extension(MauiAppBuilder mauiAppBuilder)
    {
        public void Testowo(string url)
        {
            mauiAppBuilder.Services.AddTransient<TestDelegatingHandler>();

            mauiAppBuilder.Services
                .AddHttpClient("PanMors", client => client.BaseAddress = new Uri(url))
                .AddHttpMessageHandler<TestDelegatingHandler>();
        }
    }
}
