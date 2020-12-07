using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using static Main.Constants;
using SimpleInjector;
using PocketCastsLogin;

namespace Main
{
    public class HttpClientBasicExample
    {
        private readonly ILoginWindow loginWindow;

        public HttpClientBasicExample(ILoginWindow loginWindow)
        {
            this.loginWindow = loginWindow;
        }

        public async Task<string> Try()
        {
            var (email, password) = this.loginWindow.Show();

            var client = new HttpClient();
            var content = string.Empty;

            // Create the HttpContent for the form to be posted.
            var requestContent = new FormUrlEncodedContent(new[] {

                // nocheckin: using DI and module to enter credentials, add email and password
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("password", password)
            });

            // Get the response.
            HttpResponseMessage response = await client.PostAsync(
                "https://api.pocketcasts.com/user/login",
                requestContent);

            // Get the response content.
            HttpContent responseContent = response.Content;

            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                content = await reader.ReadToEndAsync();
                Console.WriteLine(await reader.ReadToEndAsync());
            }

            return content;
        }
    }

    public class Application
    {
        public static Container BuildContainer()
        {
            // 1. Create a new Simple Injector container
            var container = new Container();

            // 2. Configure the container (register)
            container.Register<ILoginWindow, LoginWindow>(Lifestyle.Transient);
            container.Register<HttpClientBasicExample>();

            // 3. Optionally verify the container's configuration.
            container.Verify();

            return container;
        }
    }

    class Program
    {
        static async Task Main(string[] args)
        {

            var container = Application.BuildContainer();

            // TODO:
            // - Add Polly. Between 407 proxy errors and errors with bad json data, 
            //   it's clear that I need a mechanism for retrying the call to the api.

            // Still getting 407 proxy errors for some reason..
            // await FlurlBasicExample.Try();

            var filename = $"{ProjectWorkspace}\\token.json";

            // if using DI this becomes:
            var client = container.GetInstance<HttpClientBasicExample>();

            var content = await client.Try();

            File.Create(filename).Dispose();
            File.WriteAllText(filename, content);

            new FileInfo(filename).PrintTokenData();

            Console.ReadLine();
        }
    }
}