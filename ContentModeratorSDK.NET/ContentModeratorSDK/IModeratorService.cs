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
    using System.Net.Http;

    /// <summary>
    /// Interface for moderator service
    /// </summary>
    public interface IModeratorService
    {
        #region Image Moderator
        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        Task<EvaluateImageResult> EvaluateImageAsync(ImageModeratableContent imageContent);

             

        /// <summary>
        /// Match an image against the images uploaded to the Image List
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <returns>Match response</returns>
        Task<MatchImageResult> MatchImageAsync(ImageModeratableContent imageContent);

        /// <summary>
        /// Call Evaluate Image, to determine whether the image violates any policy
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Evaluate result</returns>
        Task<ExtractTextResult> ExtractTextAsync(ImageModeratableContent imageContent, string language);


        Task<DetectFaceResult> DetectFaceAsync(ImageModeratableContent imageContent);


        #endregion Image Moderator

        #region Custom Image List Management
        /// <summary>
        /// Add an image into the Image list
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Immage add result</returns>
        Task<ImageAddResult> ImageAddAsync(ImageModeratableContent imageContent);

        Task<ImageRemoveResult> ImageRemoveAsync(ImageModeratableContent imageContent);


        /// <summary>
        /// Refresh the image Index. This api needs to be called after adding an image into the image list.
        /// </summary>
        /// <returns>Add Image response</returns>
        Task<ImageRefreshIndexResult> RefreshImageIndexAsync();

        
        Task<ImageListResult> GetAllImageIdsAsync();

        Task<ImageResetResult> ResetImageListAsync();


        #endregion Custom Image List Management

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


        /// <summary>
        /// Identify the language of the provided text
        /// </summary>
        /// <param name="textContent"></param>
        /// <returns>Identified language</returns>
        Task<IdentifyLanguageResult> IdentifyLanguageAsync(TextModeratableContent textContent);

        #endregion Text Moderator

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
