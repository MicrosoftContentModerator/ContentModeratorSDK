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
                throw new ArgumentNullException("options");
            }

            if (options.HostUrl == null)
            {
                throw new ArgumentNullException("options");
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
                string urlPath = string.Format("{0}/Image/EvaluateImage",this.options.ImageServicePath);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.ImageServiceKey);

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

                return await ServiceHelpers.SendRequest<EvaluateImageResult>(client, message);
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
                string urlPath = string.Format("{0}/Image/Match", this.options.ImageServicePath);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.ImageServiceKey);

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

                
                string urlPath = string.Format("{0}/Text/Screen?language={1}&subscription-key={2}",
                    this.options.TextServicePath, language, this.options.TextServiceKey);
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.TextServiceKey);
                message.Headers.Add("x-contentsources", this.options.TextContentSourceId);
                ScreenTextRequest request = new ScreenTextRequest(textContent);
                message.Content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");
                return await ServiceHelpers.SendRequest<ScreenTextResult>(client, message);
            }
        }

        
        public async Task<IdentifyLanguageResult> IdentifyLanguageAsync(TextModeratableContent textContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);

                //string urlPath = $"{this.options.TextServicePath}{$"/Text/IdentifyLanguage?subscription-key={this.options.TextServiceCustomListKey}"}";
            string urlPath = string.Format("{0}/Language/Identify?subscription-key={1}", this.options.TextServicePath,this.options.TextServiceCustomListKey);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

              //  ServiceHelpers.Addkey(message, this.options.TextServiceKey);
                message.Headers.Add("x-contentsources", this.options.TextContentSourceId);
                IdentifyLanguageRequest request = new IdentifyLanguageRequest(textContent);
                message.Content = new StringContent(
                    JsonConvert.SerializeObject(request),
                    Encoding.UTF8,
                    "application/json");
                return await ServiceHelpers.SendRequest<IdentifyLanguageResult>(client, message);
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

                //string urlPath = $"{this.options.ImageServiceCustomListPath}{"/Image/Add"}";
                string urlPath = string.Format("{0}/Image/Add", this.options.ImageServiceCustomListPath);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.ImageServiceCustomListKey);

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

                return await ServiceHelpers.SendRequest<ImageAddResult>(client, message);
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

                //string urlPath = $"{this.options.ImageServiceCustomListPath}{"/HashIndex/Refresh"}";
                string urlPath = string.Format("{0}/HashIndex/Refresh",this.options.ImageServiceCustomListPath);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.ImageServiceCustomListKey);
                return await ServiceHelpers.SendRequest<ImageRefreshIndexResult>(client, message);
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
                string urlPath = string.Format("{0}/Image/ExtractText?language={1}", this.options.ImageServicePath, language);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.ImageServiceKey);

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

                return await ServiceHelpers.SendRequest<ExtractTextResult>(client, message);
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

                //string urlPath =
                    //$"{this.options.TextServiceCustomListPath}{$"/Text/Add/{textContent.ContentAsString}?language={language}&subscription-key={this.options.TextServiceCustomListKey}"}";
                string urlPath = string.Format("{0}/{1}?language={2}&subscription-key={3}",
                    this.options.TextServiceCustomListPath, textContent.ContentAsString, language,
                    this.options.TextServiceCustomListKey);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                //ServiceHelpers.Addkey(message, this.options.TextServiceCustomListKey);

                ServiceHelpers.AddTextContentSourceKey(message, this.options.TextContentSourceId);

                HttpResponseMessage response = await ServiceHelpers.SendRequest(client, message);

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
                
                string urlPath = string.Format("{0}/Refreshindex?language={1}",
                    this.options.TextServiceCustomListPath, language);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.TextServiceCustomListKey);

                return await ServiceHelpers.SendRequest<TextRefreshIndexResult>(client, message);
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

                //string urlPath =
                //    $"{this.options.TextServiceCustomListPath}{$"/Text/{textContent.ContentAsString}?language={language}&subscription-key={this.options.TextServiceCustomListKey}"}";
                string urlPath = string.Format("{0}/{1}?language={2}&subscription-key={3}",
                    this.options.TextServiceCustomListPath, textContent.ContentAsString, language,
                    this.options.TextServiceCustomListKey);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Delete, urlPath);

                // ServiceHelpers.Addkey(message, this.options.TextServiceCustomListKey);
                ServiceHelpers.AddTextContentSourceKey(message, this.options.TextContentSourceId);

                return await ServiceHelpers.SendRequest(client, message);
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

                //string urlPath = $"{this.options.TextServiceCustomListPath}{$"/Text/Import?language={language}&subscription-key={this.options.TextServiceCustomListKey}"}";
                string urlPath = string.Format("{0}/Import?language={1}&subscription-key={2}",
                    this.options.TextServiceCustomListPath, language, this.options.TextServiceCustomListKey);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                //ServiceHelpers.Addkey(message, this.options.TextServiceCustomListKey);
                ServiceHelpers.AddTextContentSourceKey(message, this.options.TextContentSourceId);


                return await ServiceHelpers.SendRequest<ImportTermListResult>(client, message);
            }
        }

        public async Task<DetectFaceResult> DetectFaceAsync(ImageModeratableContent imageContent)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.options.HostUrl);                
                string urlPath = string.Format("{0}/Image/DetectFaces", this.options.ImageServicePath);
                HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, urlPath);

                ServiceHelpers.Addkey(message, this.options.ImageServiceKey);
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

                return await ServiceHelpers.SendRequest<DetectFaceResult>(client, message);
            }
        }

        public Task<ImageRemoveResult> ImageRemoveAsync(ImageModeratableContent imageContent)
        {
            throw new NotImplementedException();
        }

        public Task<ImageListResult> GetAllImageIdsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ImageResetResult> ResetImageListAsync()
        {
            throw new NotImplementedException();
        }

        public Task<TermListResult> ListTermsAsync(string language)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> RemoveAllTermsAsync(string language)
        {
            throw new NotImplementedException();
        }

        public Task<HttpResponseMessage> ResetTermListAsync(string language)
        {
            throw new NotImplementedException();
        }

        #endregion




    }
}
