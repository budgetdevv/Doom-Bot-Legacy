using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DoomBot.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            var Builder = WebAssemblyHostBuilder.CreateDefault(args);

            Builder.RootComponents.Add<App>("app");

            var Services = Builder.Services;

            //Storage

            Services.AddBlazoredLocalStorage();

            Services.AddScoped(x => new HttpClient { BaseAddress = new Uri(Builder.HostEnvironment.BaseAddress) });

            Services.AddScoped<ClientUserData>();


            await Builder.Build().RunAsync();
        }
    }
}
