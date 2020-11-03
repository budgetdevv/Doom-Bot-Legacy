using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using DoomBot.Server.Command;
using DoomBot.Server.Controllers.Attributes;
using DoomBot.Server.EM;
using DoomBot.Server.Managers;
using DoomBot.Server.Modules;
using DoomBot.Server.MongoDB;
using DoomBot.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Snowflake;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DoomBot.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static IServiceProvider ServicesProvider { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection Services)
        {
            //Services.AddControllersWithViews().AddNewtonsoftJson();

            Services.AddControllersWithViews().AddJsonOptions(x =>
             {
                 var Opt = x.JsonSerializerOptions;

                 Opt.Converters.Add(new ValueTupleFactory());

                 Opt.PropertyNamingPolicy = null;
             });

            Services.AddRazorPages();

            Services.AddSingleton<HttpClient>();

            Services.AddCors(o => o.AddPolicy("Kys", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()    
                       .AllowAnyHeader();
            }));

            //Auth

            Services.AddSingleton<AuthManager>();

            //Snowflake Engine

            Services.AddSingleton<SnowflakeEngine>();

            //Discord

            Services.AddSingleton<DiscordSocketClient>();
            Services.AddSingleton<CommandService>(); 
            Services.AddSingleton<CommandHandlingService>();
            Services.AddSingleton<DiscordRestClient>();

            //Accessors 

            Services.AddScoped<UserAccessor>();
            Services.AddSingleton<DiscordAccessor>();

            //Modules

            Services.AddSingleton<MDB>();
            Services.AddSingleton<InventoryModule>();
            Services.AddSingleton<RolePerkModule>();

            //Manager

            Services.AddSingleton<PerksManager>();
            Services.AddSingleton<EconManager>();
            Services.AddSingleton<EMManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseCors("Kys");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });

            ServicesProvider = app.ApplicationServices;

            var Client = ServicesProvider.GetService<DiscordSocketClient>();

            Client.Log += LogAsync;

            ServicesProvider.GetRequiredService<CommandService>().Log += LogAsync;

            _ = StartDiscord(Client, ServicesProvider.GetService<DiscordRestClient>());
        }

        private async Task StartDiscord(DiscordSocketClient Client, DiscordRestClient RClient)
        {
            // Tokens should be considered secret data and never hard-coded.
            // We can read from the environment variable to avoid hardcoding.

            //await Client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("token"));

            await Client.LoginAsync(TokenType.Bot, "NzY5MTk5NjUzNjY0OTgwOTky.X5LjAA.yHBbHuu_puEUz3a1s9yfLc7I2Rc");
            await Client.StartAsync();

            await RClient.LoginAsync(TokenType.Bot, "NzY5MTk5NjUzNjY0OTgwOTky.X5LjAA.yHBbHuu_puEUz3a1s9yfLc7I2Rc");

            // Here we initialize the logic required to register our commands.
            await ServicesProvider.GetRequiredService<CommandHandlingService>().InitializeAsync();
        }

        private Task LogAsync(LogMessage Log)
        {
            Console.WriteLine(Log.ToString());

            return Task.CompletedTask;
        }

        /*

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .AddSingleton<PictureService>()
                .BuildServiceProvider();
        }

        */
    }
}
