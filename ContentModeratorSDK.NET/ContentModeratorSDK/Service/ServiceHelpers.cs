using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service
{
    public static class ServiceHelpers
    {
        #region Support Methods

        /// <summary>
        /// Send a request to the Moderator service
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="client">Client</param>
        /// <param name="message">Message</param>
        /// <returns>Response task</returns>
        public static async Task<T> SendRequest<T>(HttpClient client, HttpRequestMessage message)
        {
            T result = default(T);

            var sendTask = await client.SendAsync(message).ContinueWith(
                async task =>
                {
                    HttpResponseMessage messageResponse = task.Result;
                    //messageResponse.EnsureSuccessStatusCode();

                    await messageResponse.Content.ReadAsStringAsync().ContinueWith
                        (readAsyncTask => { result = GetResultObject<T>(readAsyncTask.Result); });
                });

            sendTask.Wait();

            return result;
        }

        public static async Task<HttpResponseMessage> SendRequest(HttpClient client, HttpRequestMessage message)
        {
            HttpResponseMessage response = await client.SendAsync(message);
            //response.EnsureSuccessStatusCode();
            return response;
        }

        /// <summary>
        /// Convert a response string to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseString"></param>
        /// <returns></returns>
        public static T GetResultObject<T>(string responseString)
        {
            T result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }

        /// <summary>
        /// Add the subscription key
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        public static void Addkey(HttpRequestMessage message, string key)
        {
            message.Headers.Add("Ocp-Apim-Subscription-Key", key);
        }

        public static void AddTextContentSourceKey(HttpRequestMessage message, string key)
        {
            message.Headers.Add("cs-id", key);
        }

        #endregion
    }
}
