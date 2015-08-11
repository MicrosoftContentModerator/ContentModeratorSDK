// -----------------------------------------------------------------------
//  <copyright file="MatchImageRequest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Requests
{
    using ContentModeratorSDK.Image;

    /// <summary>
    /// Request to Match an Image against an image in the image list
    /// </summary>
    public class MatchImageRequest : BaseImageRequest
    {
        public MatchImageRequest(ImageModeratableContent content)
            : base(content)
        {
        }
    }
}
