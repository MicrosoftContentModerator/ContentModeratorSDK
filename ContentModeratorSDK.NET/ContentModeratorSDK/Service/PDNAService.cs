using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentModeratorSDK.Image;
using ContentModeratorSDK.Service.Results;
using System.Net.Http;
using ContentModeratorSDK.Service.Requests;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ContentModeratorSDK.Service
{
    public class PDNAService : IPDNAService
    {

        /// <summary>
        /// Moderator Service options
        /// </summary>
        private readonly PDNAServiceOptions options;

        /// <summary>
        /// Main constructor for the Moderator Service
        /// </summary>
        /// <param name="options">Request options</param>
        public PDNAService(PDNAServiceOptions options)
        {
            this.ValidateOptions(options);
            this.options = options;
        }

        /// <summary>
        /// Validate request options
        /// </summary>
        /// <param name="options"></param>
        private void ValidateOptions(PDNAServiceOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            if (options.HostUrl == null)
            {
                throw new ArgumentNullException("options");
            }
        }

        #region PDNA Apis
        /// <summary>
        /// Validate an image against the images in the PDNA DB
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <param name="cacheContent">Cache Image content</param>
        /// <returns>Match response</returns>
        public async Task<MatchImageResult> ValidateImageAsync(ImageModeratableContent imageContent, bool cacheContent = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                string urlPath = string.Format("{0}/Validate{1}", this.options.PDNAImageServicePath,
                    cacheContent ? "?cacheImage=true" : string.Empty);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.PDNAImageServiceKey);
                MatchImageRequest request = new MatchImageRequest(imageContent);
                if (imageContent.BinaryContent == null)
                {
                    message.Content = new StringContent(
                        JsonConvert.SerializeObject(request),
                        Encoding.UTF8,
                        "application/json");
                }
                else
                {
                    message.Content = new StreamContent(imageContent.BinaryContent.Stream);
                    message.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(imageContent.BinaryContent.ContentType);
                }

                return await ServiceHelpers.SendRequest<MatchImageResult>(client, message);
            }
        }

        public async Task<MatchImageResult> ValidateImageInCache(string cacheId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);            
                string urlPath = string.Format("{0}/Validate?CacheID={1}", this.options.PDNAImageServicePath, cacheId);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, urlPath);

                ServiceHelpers.Addkey(message, this.options.PDNAImageServiceKey);
                return await ServiceHelpers.SendRequest<MatchImageResult>(client, message);
            }
        }
        #endregion
    }
}
