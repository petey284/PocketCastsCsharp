using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Flurl.Http.Configuration;

namespace Main
{
    // Got main guts from SO here: https://bit.ly/3mP0rMT
    internal class ProxyHttpClientFactory : DefaultHttpClientFactory
    {
        private readonly string _address;

        public ProxyHttpClientFactory()
        {
            // Gets current configuration for proxy address
            _address = Environment.GetEnvironmentVariable("HTTP_PROXY");
        }

        public override HttpMessageHandler CreateMessageHandler()
        {
            return new HttpClientHandler
            {
                Proxy = new WebProxy(_address),
                UseProxy = true
            };
        }
    }

    public static class FlurlBasicExample
    {
        public static async Task Try()
        {
            FlurlHttp.Configure(settings =>
            {
                settings.HttpClientFactory = new ProxyHttpClientFactory();
            });

            var result = default(string);
            try
            {
                result = await "https://api.pocketcasts.com/user/login"
                    .WithHeaders(new { origin = "https://play.pocketcasts.com" })

                    // Add way to enter password here:
                    .PostJsonAsync(new { email = "nothing", password = "nothing", scope = "webplayer" })
                    .ReceiveJson<string>();

            }
            catch (FlurlHttpException e)
            {
                // Still getting 407 proxy errors for some reason.
                Console.WriteLine(e);
                Console.WriteLine("Proxy error.");
            }

            Console.WriteLine(result);
        }
    }
}