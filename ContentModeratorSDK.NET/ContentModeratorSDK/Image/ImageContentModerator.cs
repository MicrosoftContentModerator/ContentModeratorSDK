// -----------------------------------------------------------------------
//  <copyright file="ImageContentModerator.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK
{
    using System;
    using System.Threading.Tasks;
    using ContentModeratorSDK.Image;

    /// <summary>
    /// Class handling moderation for images
    /// </summary>
    public class ImageContentModerator : IContentModerator
    {
        public async Task<IModeratorResult> Moderate(IModeratableContent content, IModeratorService service)
        {
            var imageContent = content as ImageModeratableContent;
            if (imageContent == null)
            {
                throw new ArgumentException("Content should be of valid type ImageModeratableContent");
            }

            var result = await service.EvaluateImageAsync(imageContent);

            return result;
        }
    }
}
