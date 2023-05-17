using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;




namespace KlitechHF.Services
{
    public static class HttpService
    {
        /// <summary>
        /// Requests an endpoint without query params
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <returns>T result</returns>
        public static async Task<T> GetTAsync<T>(Uri uri)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                
                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }




        /// <summary>
        /// Requests an endpoint with query params
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uri"></param>
        /// <param name="queryParams">Key-value pairs</param>
        /// <returns>T result</returns>
        public static async Task<T> GetTAsync<T>(Uri uri, Dictionary<string, string> queryParams)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(QueriedUri(uri, queryParams));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                T result = JsonConvert.DeserializeObject<T>(json);
                return result;
            }
        }




        /// <summary>
        /// Attaches the query params to the uri
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="queryParams">Key-value pairs</param>
        /// <returns>Uri with query params</returns>
        private static Uri QueriedUri(Uri uri, Dictionary<string, string> queryParams)
        {
            if (queryParams.Count == 0)
            {
                return uri;
            }

            var paramedUri = new UriBuilder(uri);
            var query = HttpUtility.ParseQueryString(paramedUri.Query);
            foreach (var kv in queryParams)
            {
                query.Add(kv.Key, kv.Value);
            }
            paramedUri.Query = query.ToString();

            return paramedUri.Uri;
        }
    }
}
