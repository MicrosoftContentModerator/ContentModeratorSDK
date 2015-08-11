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
        /// Match an image against the images uploaded to the Image List
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <returns>Match response</returns>
        Task<MatchImageResult> MatchImageAsync(ImageModeratableContent imageContent);

        /// <summary>
        /// Add an image into the Image list
        /// </summary>
        /// <param name="imageContent">Image Content</param>
        /// <returns>Immage add result</returns>
        Task<ImageAddResult> ImageAddAsync(ImageModeratableContent imageContent);

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
    }
}
