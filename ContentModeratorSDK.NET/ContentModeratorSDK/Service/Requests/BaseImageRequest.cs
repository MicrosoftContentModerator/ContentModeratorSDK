// -----------------------------------------------------------------------
//  <copyright file="BaseImageRequest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Requests
{
    using System;
    using ContentModeratorSDK.Image;

    /// <summary>
    /// Key value pairs for passing additional meta data info.
    /// </summary>
    public class KeyValue
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    /// <summary>
    /// Base request for an image operation
    /// </summary>
    public class BaseImageRequest : BaseModeratorRequest
    {
        /// <summary>
        /// Create image request
        /// </summary>
        /// <param name="imageContent"></param>
        public BaseImageRequest(ImageModeratableContent imageContent)
        {
            if (imageContent == null)
            {
                throw new ArgumentNullException("imageContent");
            }

            this.DataRepresentation = imageContent.DataRepresentation;
            this.Value = imageContent.ContentAsString;
        }

        /// <summary>
        /// Image data representation
        /// </summary>
        public string DataRepresentation { private set; get; }

        /// <summary>
        /// Image content
        /// </summary>
        public string Value { private set; get; }
    }
}
