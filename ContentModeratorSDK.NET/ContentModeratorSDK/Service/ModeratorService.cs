// -----------------------------------------------------------------------
//  <copyright file="ModeratorService.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using ContentModeratorSDK.Image;
    using ContentModeratorSDK.Service.Requests;
    using ContentModeratorSDK.Service.Requests.TermList;
    using ContentModeratorSDK.Service.Results;
    using ContentModeratorSDK.Text;
    using Newtonsoft.Json;
    using System.Collections.Generic;

    /// <summary>
    /// Service which interacts with the Content Moerator Api
    /// </summary>
    public class ModeratorService : IModeratorService
    {
        /// <summary>
        /// Moderator Service options
        /// </summary>
        private readonly ModeratorServiceOptions options;

        /// <summary>
        /// Main constructor for the Moderator Service
        /// </summary>
        /// <param name="options">Request options</param>
        public ModeratorService(ModeratorServiceOptions options)
        {
            this.ValidateOptions(options);
            this.options = options;
        }

        /// <summary>
        /// Validate request options
        /// </summary>
        /// <param name="options"></param>
        private void ValidateOptions(ModeratorServiceOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), "Options parameter is null");
            }

            if (options.HostUrl == null)
            {
                throw new ArgumentNullException(nameof(options), "Options.HostUrl is null");
            }
        }

        #region Apis

        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy. This api returns a single
        /// score. 
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        public async Task<EvaluateImageResult> EvaluateImageAsync(ImageModeratableContent imageContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                //string urlPath = $"{this.options.ImageServicePath}{"/Image/EvaluateImage"}";
                string urlPath = $"{this.options.ImageServicePath}{"/Evaluate"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);

                EvaluateImageRequest request = new EvaluateImageRequest(imageContent);

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

                return await this.SendRequest<EvaluateImageResult>(client, message);
            }
        }

        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        public async Task<EvaluateImageResult> EvaluateImageWithMultipleRatingsAsync(ImageModeratableContent imageContent, bool cacheContent = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                //string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Image/EvaluateImage{0}", cacheContent ? "?cacheImage=true" : string.Empty)}";
                string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Evaluate{0}", cacheContent ? "?cacheImage=true" : string.Empty)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);

                EvaluateImageRequest request = new EvaluateImageRequest(imageContent);

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

                return await this.SendRequest<EvaluateImageResult>(client, message);
            }
        }

        public async Task<EvaluateImageResult> EvaluateImageInCache(string cacheId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Evaluate?CacheID={0}", cacheId)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);
                return await this.SendRequest<EvaluateImageResult>(client, message);
            }
        }        

        /// <summary>
        /// Match an image against the images uploaded to the Image List
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <returns>Match response</returns>
        public async Task<MatchImageResult> MatchImageAsync(ImageModeratableContent imageContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                //string urlPath = $"{this.options.ImageServicePath}{"/Image/Match"}";
                string urlPath = $"{this.options.ImageServicePath}{"/Match"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);

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

                return await this.SendRequest<MatchImageResult>(client, message);
            }
        }

        /// <summary>
        /// Match an image against the images uploaded to the Image List
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <param name="cacheContent">Cache Image content</param>
        /// <returns>Match response</returns>
        public async Task<MatchImageResult> MatchImageAsyncV2(ImageModeratableContent imageContent, bool cacheContent = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                //string urlPath = $"{this.options.ImageServicePathV2}{"/Image/Match"}";
                //string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Image/Match{0}", cacheContent ? "?cacheImage=true" : string.Empty)}";
                string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Match{0}", cacheContent ? "?cacheImage=true" : string.Empty)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);
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

                return await this.SendRequest<MatchImageResult>(client, message);
            }
        }

        /// <summary>
        /// Screen Text against the term list. Note that Import Term List needs to be run
        /// against the appropriate language before calling this method, for all the terms
        /// of that language to have been imported.
        /// </summary>
        /// <param name="textContent">Text to screen</param>
        /// <param name="language">Language in ISO-639-3 format</param>
        /// <returns></returns>
        public async Task<ScreenTextResult> ScreenTextAsync(TextModeratableContent textContent, string language = "eng")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath = $"{this.options.TextServicePath}{$"/Text/Screen?language={language}&subscription-key={this.options.TextServiceCustomListKey}"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                // this.Addkey(message, this.options.TextServiceKey);
                message.Headers.Add("x-contentsources", this.options.TextContentSourceId);
                ScreenTextRequest request = new ScreenTextRequest(textContent);
                message.Content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");
                return await this.SendRequest<ScreenTextResult>(client, message);
            }
        }

        /// <summary>
        /// Screen Text against the term list. Note that Import Term List needs to be run
        /// against the appropriate language before calling this method, for all the terms
        /// of that language to have been imported.
        /// </summary>
        /// <param name="textContent">Text to screen</param>
        /// <param name="language">Language in ISO-639-3 format</param>
        /// <returns></returns>
        public async Task<ScreenTextResult> ScreenTextV2Async(TextModeratableContent textContent, string language = "eng")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath = $"{this.options.TextServicePathV2}{$"/Screen?language={language}"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.TextServiceKey);

                message.Headers.Add("x-contentsources", this.options.TextContentSourceId);

                message.Content = new StringContent(
                    textContent.ContentAsString,
                    Encoding.Default,
                    "text/plain");
                message.Content.Headers.ContentType.MediaType = "text/plain";
                message.Content.Headers.ContentType.CharSet = null;

                return await this.SendRequest<ScreenTextResult>(client, message);
            }
        }

        public async Task<IdentifyLanguageResult> IdentifyLanguageAsync(TextModeratableContent textContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath = $"{this.options.TextServicePath}{$"/Text/IdentifyLanguage?subscription-key={this.options.TextServiceCustomListKey}"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

              //  this.Addkey(message, this.options.TextServiceKey);
                message.Headers.Add("x-contentsources", this.options.TextContentSourceId);
                IdentifyLanguageRequest request = new IdentifyLanguageRequest(textContent);
                message.Content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");
                return await this.SendRequest<IdentifyLanguageResult>(client, message);
            }
        }

        /// <summary>
        /// Add an image into the Image list
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <param name="labels">Image labels</param>
        /// <returns>Immage add result</returns>
        public async Task<ImageAddResult> ImageAddAsync(ImageModeratableContent imageContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath = $"{this.options.ImageServiceCustomListPath}{"/Image/Add"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceCustomListKey);

                ImageAddRequest request = new ImageAddRequest(imageContent);
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

                return await this.SendRequest<ImageAddResult>(client, message);
            }
        }

        /// <summary>
        /// Add an image into the Image list
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <param name="tag">Image policies</param>
        /// <param name="label">Image description</param>
        /// <returns>Immage add result</returns>
        public async Task<ImageAddResult> ImageAddAsyncV2(ImageModeratableContent imageContent, string tag, string label)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string queryParam = string.IsNullOrWhiteSpace(tag) ? string.Empty : "?tag=" + tag;
                if (string.IsNullOrWhiteSpace(queryParam))
                    queryParam = string.IsNullOrWhiteSpace(label) ? string.Empty : "?label=" + label;
                else
                    queryParam = queryParam + "&label=" + label;

                //string urlPath = $"{this.options.ImageServiceCustomListPathV2}{"/Add"}";
                string urlPath = $"{this.options.ImageServiceCustomListPathV2}{string.Format("/Image/Add{0}", string.IsNullOrWhiteSpace(queryParam) ? string.Empty : queryParam)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceCustomListKeyV2);

                ImageAddRequest request = new ImageAddRequest(imageContent);
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

                return await this.SendRequest<ImageAddResult>(client, message);
            }
        }

        /// <summary>
        /// Refresh the image Index. This api needs to be called after adding an image into the image list.
        /// </summary>
        /// <returns>Add Image response</returns>
        public async Task<ImageRefreshIndexResult> RefreshImageIndexAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath = $"{this.options.ImageServiceCustomListPath}{"/HashIndex/Refresh"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceCustomListKey);
                return await this.SendRequest<ImageRefreshIndexResult>(client, message);
            }
        }

        /// <summary>
        /// Refresh the image Index. This api needs to be called after adding an image into the image list.
        /// </summary>
        /// <returns>Add Image response</returns>
        public async Task<ImageRefreshIndexResult> RefreshImageIndexV2Async()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath = $"{this.options.ImageServiceCustomListPathV2}{"/HashIndex/Refresh"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceCustomListKeyV2);
                return await this.SendRequest<ImageRefreshIndexResult>(client, message);
            }
        }

        /// <summary>
        /// Call ExtractText method to extract the text in an image through the use of OCR
        /// </summary>
        /// <param name="imageContent">Image to run Extraction on</param>
        /// <param name="language">Language</param>
        /// <returns>Extraction result</returns>
        public async Task<ExtractTextResult> ExtractTextAsync(ImageModeratableContent imageContent, string language = "eng")
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                //string urlPath = $"{this.options.ImageServicePath}{"/Image/ExtractText"}{"?language="}{language}";
                string urlPath = $"{this.options.ImageServicePath}{"/OCR"}{"?language="}{language}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);

                ExtractTextRequest request = new ExtractTextRequest(imageContent);

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

                return await this.SendRequest<ExtractTextResult>(client, message);
            }
        }

        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        public async Task<ExtractTextResult> ExtractTextAsyncV2(ImageModeratableContent imageContent, string language = "eng", bool cacheContent = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                //string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Image/DetectTextEnhanced{0}", cacheContent ? "?cacheImage=true" : string.Empty)}{"&language="}{language}";
                //string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/OCR{0}", cacheContent ? "?cacheImage=true" : string.Empty)}{"&language="}{language}";
                string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/OCR?language={0}{1}", language, cacheContent ? "&cacheImage=true" : string.Empty)}";//{"&language="}{language}
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);
                message.Headers.Add("language", "eng");
                //message.Headers.Add("x-contentsources", "3061");
                ExtractTextRequest request = new ExtractTextRequest(imageContent);

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

                return await this.SendRequest<ExtractTextResult>(client, message);
            }
        }

        public async Task<ExtractTextResult> ExtractTextInCache(string cacheId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                //string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Image/DetectTextEnhanced?CacheID={0}", cacheId)}";
                string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/OCR?CacheID={0}", cacheId)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);
                return await this.SendRequest<ExtractTextResult>(client, message);
            }
        }

        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        public async Task<DetectFaceResult> DetectFaceAsync(ImageModeratableContent imageContent, bool cacheContent = false)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                //string urlPath = $"{this.options.ImageServicePath}{string.Format("/Image/DetectFaces{0}", cacheContent ? "?cacheImage=true" : string.Empty)}";
                string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Faces{0}", cacheContent ? "?cacheImage=true" : string.Empty)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);
                DetectFaceRequest request = new DetectFaceRequest(imageContent);

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

                return await this.SendRequest<DetectFaceResult>(client, message);
            }
        }

        public async Task<DetectFaceResult> DetectFaceInCache(string cacheId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Faces?CacheID={0}", cacheId)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, urlPath);

                this.Addkey(message, this.options.ImageServiceKey);
                return await this.SendRequest<DetectFaceResult>(client, message);
            }
        }

        public async Task<BaseModeratorResult> CacheImageContent(ImageModeratableContent imageContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                string urlPath = $"{this.options.ImageCachingPath}{string.Format("/")}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.ImageCachingKey);
                
                BaseImageRequest request = new BaseImageRequest(imageContent);

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

                return await this.SendRequest<BaseModeratorResult>(client, message);
            }
        }

        public async Task<BaseModeratorResult> UnCacheImageContent(string cacheId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                //string urlPath = $"{this.options.ImageCachingPath}{string.Format("/Image/Cache")}";
                string urlPath = $"{this.options.ImageCachingPath}{string.Format("?CacheID={0}", cacheId)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, urlPath);

                this.Addkey(message, this.options.ImageCachingKey);

                return await this.SendRequest<BaseModeratorResult>(client, message);
            }
        }

        /// <summary>
        /// Adds a term to the term list
        /// </summary>
        /// <param name="textContent">Term text</param>
        /// <param name="language">Term language</param>
        /// <returns>Add Term Response</returns>
        public async Task<HttpResponseMessage> AddTermAsync(TextModeratableContent textContent, string language)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath =
                    $"{this.options.TextServiceCustomListPath}{$"/Text/Add/{textContent.ContentAsString}?language={language}&subscription-key={this.options.TextServiceCustomListKey}"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                //this.Addkey(message, this.options.TextServiceCustomListKey);

                this.AddTextContentSourceKey(message, this.options.TextContentSourceId);

                HttpResponseMessage response = await this.SendRequest(client, message);

                return response;

            }
        }

        /// <summary>
        /// Refresh the text index. This method needs to be called after adding a text term.
        /// </summary>
        /// <param name="language">Term language</param>
        /// <returns>Add term response</returns>
        public async Task<TextRefreshIndexResult> RefreshTextIndexAsync(string language)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath =
                    $"{this.options.TextServiceCustomListPath}{$"/Text/Refreshindex?language={language}"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.TextServiceCustomListKey);

                return await this.SendRequest<TextRefreshIndexResult>(client, message);
            }
        }

        /// <summary>
        /// Remove a term from the term list
        /// </summary>
        /// <param name="textContent">Text content</param>
        /// <param name="language">Language of term to remove</param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> RemoveTermAsync(TextModeratableContent textContent, string language)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath =
                    $"{this.options.TextServiceCustomListPath}{$"/Text/{textContent.ContentAsString}?language={language}&subscription-key={this.options.TextServiceCustomListKey}"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, urlPath);

                // this.Addkey(message, this.options.TextServiceCustomListKey);
                this.AddTextContentSourceKey(message, this.options.TextContentSourceId);

                return await this.SendRequest(client, message);
            }
        }

        /// <summary>
        /// Import term list. This is required to match against the default term list for a given language
        /// </summary>
        /// <param name="language">Term language</param>
        /// <returns>Match result</returns>
        public async Task<ImportTermListResult> ImportTermListAsync(string language)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                string urlPath = $"{this.options.TextServiceCustomListPath}{$"/Text/Import?language={language}&subscription-key={this.options.TextServiceCustomListKey}"}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                //this.Addkey(message, this.options.TextServiceCustomListKey);
                this.AddTextContentSourceKey(message, this.options.TextContentSourceId);


                return await SendRequest<ImportTermListResult>(client, message);
            }
        }

        #endregion

        #region Support Methods

        /// <summary>
        /// Send a request to the Moderator service
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="client">Client</param>
        /// <param name="message">Message</param>
        /// <returns>Response task</returns>
        private async Task<T> SendRequest<T>(HttpClient client, HttpRequestMessage message)
        {
            T result = default(T);

            var sendTask = await client.SendAsync(message).ContinueWith(
                async task =>
                {
                    HttpResponseMessage messageResponse = task.Result;
                    messageResponse.EnsureSuccessStatusCode();

                    await messageResponse.Content.ReadAsStringAsync().ContinueWith
                        (readAsyncTask => { result = this.GetResultObject<T>(readAsyncTask.Result); });
                });

            sendTask.Wait();

            return result;
        }

        private async Task<HttpResponseMessage> SendRequest(HttpClient client, HttpRequestMessage message)
        {
            HttpResponseMessage response = await client.SendAsync(message);
            response.EnsureSuccessStatusCode();
            return response;
        }

        /// <summary>
        /// Convert a response string to an object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="responseString"></param>
        /// <returns></returns>
        private T GetResultObject<T>(string responseString)
        {
            T result = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }

        /// <summary>
        /// Add the subscription key
        /// </summary>
        /// <param name="message"></param>
        /// <param name="key"></param>
        private void Addkey(HttpRequestMessage message, string key)
        {
            message.Headers.Add("Ocp-Apim-Subscription-Key", key);
        }

        private void AddTextContentSourceKey(HttpRequestMessage message, string key)
        {
            message.Headers.Add("cs-id", key);
        }

        #endregion

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

                //string urlPath = $"{this.options.ImageServicePathV2}{"/Image/Match"}";
                //string urlPath = $"{this.options.ImageServicePathV2}{string.Format("/Image/Match{0}", cacheContent ? "?cacheImage=true" : string.Empty)}";
                string urlPath = $"{this.options.PDNAImageServicePath}{string.Format("/Validate{0}", cacheContent ? "?cacheImage=true" : string.Empty)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                this.Addkey(message, this.options.PDNAImageServiceKey);
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

                return await this.SendRequest<MatchImageResult>(client, message);
            }
        }

        public async Task<MatchImageResult> ValidateImageInCache(string cacheId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);
                string urlPath = $"{this.options.PDNAImageServicePath}{string.Format("/Validate?CacheID={0}", cacheId)}";
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Get, urlPath);

                this.Addkey(message, this.options.PDNAImageServiceKey);
                return await this.SendRequest<MatchImageResult>(client, message);
            }
        }
        #endregion
    }
}
