// -----------------------------------------------------------------------
//  <copyright file="IModeratorService.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK
{
    using System.Threading.Tasks;
    using ContentModeratorSDK.Image;
    using ContentModeratorSDK.Service.Results;
    using ContentModeratorSDK.Text;

    /// <summary>
    /// Interface for moderator service
    /// </summary>
    public interface IModeratorService
    {
        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        Task<EvaluateImageResult> EvaluateImageAsync(ImageModeratableContent imageContent);

        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy based
        ///  on multiple ratings.
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        Task<EvaluateImageResult> EvaluateImageWithMultipleRatingsAsync(ImageModeratableContent imageContent, bool cacheContent = false);

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
        /// <returns>Match response</returns>
        Task<MatchImageResult> MatchImageAsync(ImageModeratableContent imageContent);

        /// <summary>
        /// Match an image against the images uploaded to the Image List
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <param name="cacheContent">Cache Image content</param>
        /// <returns>Match response</returns>
        Task<MatchImageResult> MatchImageAsyncV2(ImageModeratableContent imageContent, bool cacheContent = false);

        /// <summary>
        /// Add an image into the Image list
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Immage add result</returns>
        Task<ImageAddResult> ImageAddAsync(ImageModeratableContent imageContent);

        /// <summary>
        /// Add an image into the Image list
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <param name="tag">Image policies</param>
        /// <param name="label">Image description</param>
        /// <param name="csId">CustomSourceId</param>
        /// <returns>Immage add result</returns>
        Task<ImageAddResult> ImageAddAsyncV2(ImageModeratableContent imageContent, string tag, string label);

        /// <summary>
        /// Screen Text against the term list. Note that Import Term List needs to be run
        /// against the appropriate language before calling this method, for all the terms
        /// of that language to have been imported.
        /// </summary>
        /// <param name="textContent">Text to screen</param>
        /// <param name="language">Language in ISO-639-3 format</param>
        /// <returns></returns>
        Task<ScreenTextResult> ScreenTextV2Async(TextModeratableContent textContent, string language);

        /// <summary>
        /// Screen Text against the term list. Note that Import Term List needs to be run
        /// against the appropriate language before calling this method, for all the terms
        /// of that language to have been imported.
        /// </summary>
        /// <param name="textContent">Text to screen</param>
        /// <param name="language">Language in ISO-639-3 format</param>
        /// <returns></returns>
        Task<ScreenTextResult> ScreenTextAsync(TextModeratableContent textContent, string language);

        /// <summary>
        /// Refresh the image Index. This api needs to be called after adding an image into the image list.
        /// </summary>
        /// <returns>Add Image response</returns>
        Task<ImageRefreshIndexResult> RefreshImageIndexAsync();

        /// <summary>
        /// Refresh the image Index. This api needs to be called after adding an image into the image list.
        /// </summary>
        /// <returns>Add Image response</returns>
        Task<ImageRefreshIndexResult> RefreshImageIndexV2Async();

        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        Task<ExtractTextResult> ExtractTextAsync(ImageModeratableContent imageContent, string language);

        /// <summary>
        /// Call Detect Text enhanced, to determine whether the image contains any text violates any policy based
        ///  on multiple ratings.
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Extract text result</returns>
        Task<ExtractTextResult> ExtractTextAsyncV2(ImageModeratableContent imageContent, string language = "eng", bool cacheContent = false);

        /// <summary>
        /// Call Detect Text enhanced in Cache, to determine whether the image contains any text violates any policy based
        ///  on multiple ratings.
        /// </summary>
        /// <param name="cacheId">Cache Id</param>
        /// <returns>Extract text result</returns>
        Task<ExtractTextResult> ExtractTextInCache(string cacheId);

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

        ///// <summary>
        ///// Call Detect faces in Cache.
        ///// </summary>
        ///// <param name="cacheId">Cache Id</param>
        ///// <returns>Detect Face result</returns>
        //Task<DetectFaceResult> DetectFaceInCache(string cacheId);

        /// <summary>
        /// Adds a term to the term list
        /// </summary>
        /// <param name="textContent">Term text</param>
        /// <param name="language">Term language</param>
        /// <returns>Add Term Response</returns>
        Task<AddTermResult> AddTermAsync(TextModeratableContent textContent, string language);

        /// <summary>
        /// Refresh the text index. This method needs to be called after adding a text term.
        /// </summary>
        /// <param name="language">Term language</param>
        /// <returns>Add term response</returns>
        Task<TextRefreshIndexResult> RefreshTextIndexAsync(string language);

        /// <summary>
        /// Remove a term from the term list
        /// </summary>
        /// <param name="textContent">Text content</param>
        /// <param name="language">Language of term to remove</param>
        /// <returns></returns>
        Task<RemoveTermResult> RemoveTermAsync(TextModeratableContent textContent, string language);

        Task<ImportTermListResult> ImportTermListAsync(string language);

        /// <summary>
        /// Identify the language of the provided text
        /// </summary>
        /// <param name="textContent"></param>
        /// <returns>Identified language</returns>
        Task<IdentifyLanguageResult> IdentifyLanguageAsync(TextModeratableContent textContent);

        /// <summary>
        /// Cache Image content for re-use.
        /// </summary>
        /// <param name="imageContent"></param>
        /// <returns></returns>
        Task<BaseModeratorResult> CacheImageContent(ImageModeratableContent imageContent);

        /// <summary>
        /// Cache Image content for re-use.
        /// </summary>
        /// <param name="imageContent"></param>
        /// <returns></returns>
        Task<BaseModeratorResult> UnCacheImageContent(string cacheId);


        //PDNA Methods

        /// <summary>
        /// Validate an image against the images in the PDNA DB
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <param name="cacheContent">Cache Image content</param>
        /// <returns>Match response</returns>
        Task<MatchImageResult> ValidateImageAsync(ImageModeratableContent imageContent, bool cacheContent = false);

        /// <summary>
        /// Call Validate Image in Cache, against the images in the PDNA DB
        ///  on multiple ratings.
        /// </summary>
        /// <param name="cacheId">cached image id</param>
        /// <returns>Evaluate result</returns>
        Task<MatchImageResult> ValidateImageInCache(string cacheId);
    }
}
