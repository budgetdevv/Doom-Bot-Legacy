using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
//using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Serializer.ValueTuple;

namespace DoomBot.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //BsonSerializer.RegisterGenericSerializerDefinition(typeof(Tuple<>), typeof(TupleSerializer<>));

            //BsonSerializer.RegisterGenericSerializerDefinition(typeof(Tuple<,>), typeof(TupleSerializer<,>));

            //BsonSerializer.RegisterGenericSerializerDefinition(typeof(Tuple<,,>), typeof(TupleSerializer<,,>));

            //BsonSerializer.RegisterGenericSerializerDefinition(typeof(Tuple<,,,>), typeof(TupleSerializer<,,,>));

            ValueTupleSerializerRegistry.Register();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://*", "https://*");
                    
                    //webBuilder.UseStartup<Startup>();
                });
    }
}
