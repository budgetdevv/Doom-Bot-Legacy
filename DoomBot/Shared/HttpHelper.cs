using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
namespace DoomBot.Shared
{
    public static class HttpHelper
    {
        //This class is thread safe

        public const string APIEndpoint = "https://insidediscord.xyz/";

        //public const string APIEndpoint = "https://localhost/";

        private static readonly JsonSerializerOptions Opt;

        private static readonly MemoryStream MS;

        private static readonly SemaphoreSlim Lock;

        static HttpHelper()
        {
            Opt = new JsonSerializerOptions();

            Opt.Converters.Add(new ValueTupleFactory());

            Opt.PropertyNamingPolicy = null;

            MS = new MemoryStream();

            Lock = new SemaphoreSlim(1, 1);
        }

    public static async Task<T> GetAs<T>(this HttpClient HC, string Endpoint)
        {
            try
            {
                var Resp = await HC.GetAsync($"{APIEndpoint}{Endpoint}");

                await Lock.WaitAsync();

                MS.SetLength(0);

                //Console.WriteLine($"{Endpoint} | {await Resp.Content.ReadAsStringAsync()}");

                await Resp.Content.CopyToAsync(MS);

                //Console.WriteLine(MS.Position);

                MS.Position = 0;

                return await JsonSerializer.DeserializeAsync<T>(MS, Opt);
            }

            catch (Exception Ex)
            {
                //Console.WriteLine("Error");

                return default;
            }

            finally
            {
                if (Lock.CurrentCount == 0)
                {
                    Lock.Release();
                }
            }
        }

        public static async Task<T> PostAs<T, F>(this HttpClient HC, string Endpoint, F Data)
        {
            try
            {
                await Lock.WaitAsync();

                MS.SetLength(0);

                await JsonSerializer.SerializeAsync(MS, Data, Opt);

                MS.Position = 0;

                var Content = new StreamContent(MS);

                Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                //Console.WriteLine(await Content.ReadAsStringAsync());

                var Resp = await HC.PostAsync($"{APIEndpoint}{Endpoint}", Content);

                MS.SetLength(0);

                await Resp.Content.CopyToAsync(MS);

                MS.Position = 0;

                return await JsonSerializer.DeserializeAsync<T>(MS, Opt);
            }

            catch (Exception Ex)
            {
                //Console.WriteLine("Error");

                return default;
            }

            finally
            {
                if (Lock.CurrentCount == 0)
                {
                    Lock.Release();
                }
            }
        }
    }
}
