using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DoomBot.Shared
{
    public static class HttpHelper
    {
        public const string APIEndpoint = "https://insidediscord.xyz/"; //Should end with a slash, unless its local

        //public const string APIEndpoint = "";

        public static StringBuilder SB = new StringBuilder();

        public static async Task<T> GetAs<T>(this HttpClient HC, string Endpoint)
        {
            SB.Clear();

            SB.Append(APIEndpoint);

            SB.Append(Endpoint);

            Endpoint = SB.ToString();

            string Content = default;

            try
            {
                var Resp = await HC.GetAsync(Endpoint);

                Content = await Resp.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<T>(Content);
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Content);

                return default;
            }
        }

        public static async Task<T> PostAs<T, F>(this HttpClient HC, string Endpoint, F Data)
        {
            SB.Clear();

            SB.Append(APIEndpoint);

            SB.Append(Endpoint);

            Endpoint = SB.ToString();

            try
            {
                var Resp = await HC.PostAsync(Endpoint, new StringContent(JsonConvert.SerializeObject(Data), Encoding.UTF8, "application/json"));

                return JsonConvert.DeserializeObject<T>(await Resp.Content.ReadAsStringAsync());
            }

            catch (Exception Ex)
            {
                Console.WriteLine(Ex);

                return default;
            }
        }
    }
}
