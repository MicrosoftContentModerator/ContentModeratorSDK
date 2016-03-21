using ContentModeratorSDK.Image;
using ContentModeratorSDK.Service.Results;
using ContentModeratorSDK.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK
{
    public interface IModeratorServiceV2
    {

        #region Image Moderator
        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy based
        ///  on multiple ratings.
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        Task<EvaluateImageResult> EvaluateImageAsync(ImageModeratableContent imageContent, bool cacheContent = false);


        /// <summary>
        /// Call Evaluate Image in Cache, to determine whether the image violates any policy based
        ///  on multiple ratings.
        /// </summary>
        /// <param name="cacheId">cached image id</param>
        /// <returns>Evaluate result</returns>
        Task<EvaluateImageResult> EvaluateImageInCache(string cacheId);

        /// <summary>
        /// Match an image against the images uploaded to the Image List
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <param name="cacheContent">Cache Image content</param>
        /// <returns>Match response</returns>
        Task<MatchImageResult> MatchImageAsync(ImageModeratableContent imageContent, bool cacheContent = false);

        /// <summary>
        /// Match an image against the images uploaded to the Image List
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <param name="cacheContent">Cache Image content</param>
        /// <returns>Match response</returns>
        Task<MatchImageResult> MatchImageInCacheAsync(string cacheId);

        /// <summary>
        /// Detect faces in the image content
        /// </summary>
        /// <param name="imageContent">Image content</param>
        /// <param name="cacheContent">Cache Image content</param>
        /// <returns>Detect Face result</returns>
        Task<DetectFaceResult> DetectFaceAsync(ImageModeratableContent imageContent, bool cacheContent = false);

        /// <summary>
        /// Call Detect Faces in Cached image content.
        /// </summary>
        /// <param name="cacheId">cached image id</param>
        /// <returns>Detect Face result</returns>
        Task<DetectFaceResult> DetectFaceInCache(string cacheId);

       
        /// <summary>
        /// Call Detect Text enhanced, to determine whether the image contains any text violates any policy based
        ///  on multiple ratings.
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Extract text result</returns>
        Task<ExtractTextResult> ExtractTextAsync(ImageModeratableContent imageContent, string language = "eng", bool cacheContent = false);


        /// <summary>
        /// Call Detect Text enhanced in Cache, to determine whether the image contains any text violates any policy based
        ///  on multiple ratings.
        /// </summary>
        /// <param name="cacheId">Cache Id</param>
        /// <returns>Extract text result</returns>
        Task<ExtractTextResult> ExtractTextInCache(string cacheId);

        #endregion Image Moderator

        #region Image Caching

        Task<BaseModeratorResult> CacheImageContent(ImageModeratableContent imageContent);
        Task<BaseModeratorResult> UnCacheImageContent(string cacheId);
        #endregion

        #region Custom Image List Managment

        Task<ImageAddResult> ImageAddAsync(ImageModeratableContent imageContent, string tag, string label);

        Task<HttpResponseMessage> CheckImageIndexStatusAsync();

        Task<ImageRefreshIndexResult> RefreshImageIndexAsync();

        #endregion

        #region Text Moderator

        /// <summary>
        /// Screen Text against the term list. Note that Import Term List needs to be run
        /// against the appropriate language before calling this method, for all the terms
        /// of that language to have been imported.
        /// </summary>
        /// <param name="textContent">Text to screen</param>
        /// <param name="language">Language in ISO-639-3 format</param>
        /// <returns></returns>
        Task<ScreenTextResult> ScreenTextAsync(TextModeratableContent textContent, string language);


        Task<IdentifyLanguageResult> IdentifyLanguageAsync(TextModeratableContent textContent);

        #endregion

        #region Custom Term List Management
        Task<HttpResponseMessage> AddTermAsync(TextModeratableContent textContent, string language);
        Task<ImportTermListResult> ImportTermListAsync(string language);

        Task<TermListResult> ListTermsAsync(string language);



        Task<TextRefreshIndexResult> RefreshTextIndexAsync(string language);

        Task<HttpResponseMessage> RemoveAllTermsAsync(string language);

        Task<HttpResponseMessage> RemoveTermAsync(TextModeratableContent textContent, string language);

        Task<HttpResponseMessage> ResetTermListAsync(string language);
        #endregion
    }
}
