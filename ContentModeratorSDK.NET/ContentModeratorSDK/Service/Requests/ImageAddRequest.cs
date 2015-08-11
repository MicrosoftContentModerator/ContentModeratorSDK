// -----------------------------------------------------------------------
//  <copyright file="ImageAddRequest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Requests
{
    using ContentModeratorSDK.Image;

    /// <summary>
    /// Image Add Request. This request supports adding an image to the image list, which
    /// can then later be used for scanning new images for matches
    /// </summary>
    public class ImageAddRequest : BaseModeratorRequest
    {
        public Content Content;

        public ImageAddRequest(ImageModeratableContent content)
        {
            this.Content = new Content()
            {
                Value = content.ContentAsString,
                DataRepresentation = content.DataRepresentation
            };
        }
    }

    /// <summary>
    /// Image content
    /// </summary>
    public class Content
    {
        public string DataRepresentation;
        public string Value;
    }
}
