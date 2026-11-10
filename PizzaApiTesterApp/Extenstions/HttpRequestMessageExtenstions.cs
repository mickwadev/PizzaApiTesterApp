using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaApiTesterApp.Extenstions;

internal static class HttpRequestMessageExtenstions
{
    extension(HttpRequestMessage orignal)
        {
        public async Task<HttpRequestMessage> CopyRequestAsync()
        {
            var clone = new HttpRequestMessage(orignal.Method, orignal.RequestUri) { Version = orignal.Version };
            if (orignal.Content != null)
            {
                var bytes = await orignal.Content.ReadAsByteArrayAsync();
                clone.Content = new ByteArrayContent(bytes);
                foreach (var h in orignal.Content.Headers)
                {
                    // content też ma swoje nagłówki
                    clone.Content.Headers.TryAddWithoutValidation(h.Key, h.Value);
                }
            }
            foreach (var h in orignal.Headers)
            {
                clone.Headers.TryAddWithoutValidation(h.Key, h.Value);
            }

            return clone;
        }
    }
}

