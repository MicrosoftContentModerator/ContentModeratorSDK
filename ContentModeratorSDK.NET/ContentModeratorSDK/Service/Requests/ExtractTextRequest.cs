// -----------------------------------------------------------------------
//  <copyright file="ExtractTextRequest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Requests
{
    using ContentModeratorSDK.Image;

    /// <summary>
    /// Request to extract text from an image
    /// </summary>
    public class ExtractTextRequest : BaseImageRequest
    {
        public ExtractTextRequest(ImageModeratableContent content)
            : base(content)
        {
        }
    }
}
