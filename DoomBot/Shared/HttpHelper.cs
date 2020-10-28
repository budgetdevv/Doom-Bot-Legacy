using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
namespace DoomBot.Shared
{
    public static class HttpHelper
    {
        //This class is NOT thread safe!

        //public const string APIEndpoint = "https://insidediscord.xyz/";

        public const string APIEndpoint = "https://localhost/";

        private static readonly StringBuilder SB = new StringBuilder();

        private static readonly JsonSerializerOptions Opt;

        private static readonly MemoryStream MS;

        static HttpHelper()
        {
            Opt = new JsonSerializerOptions();

            Opt.Converters.Add(new ValueTupleFactory());

            MS = new MemoryStream();
        }

    public static async Task<T> GetAs<T>(this HttpClient HC, string Endpoint)
        {
            SB.Clear();

            SB.Append(APIEndpoint);

            SB.Append(Endpoint);

            Endpoint = SB.ToString();

            MS.SetLength(0);

            try
            {
                var Resp = await HC.GetAsync(Endpoint);

                Console.WriteLine(await Resp.Content.ReadAsStringAsync());

                await Resp.Content.CopyToAsync(MS);

                MS.Position = 0;

                return await JsonSerializer.DeserializeAsync<T>(MS, Opt);
            }

            catch (Exception Ex)
            {
                Console.WriteLine("Error");

                return default;
            }
        }

        public static async Task<T> PostAs<T, F>(this HttpClient HC, string Endpoint, F Data)
        {
            SB.Clear();

            SB.Append(APIEndpoint);

            SB.Append(Endpoint);

            Endpoint = SB.ToString();

            MS.SetLength(0);

            try
            {
                await JsonSerializer.SerializeAsync(MS, Data, Opt);

                MS.Position = 0;

                var Resp = await HC.PostAsync(Endpoint, new StreamContent(MS));

                MS.SetLength(0);

                await Resp.Content.CopyToAsync(MS);

                MS.Position = 0;

                return await JsonSerializer.DeserializeAsync<T>(MS, Opt);
            }

            catch (Exception Ex)
            {
                Console.WriteLine("Error");

                return default;
            }
        }
    }
}
