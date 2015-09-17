// -----------------------------------------------------------------------
//  <copyright file="EvaluateImageRequest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Requests
{
    using ContentModeratorSDK.Image;
    using System.Collections.Generic;

    /// <summary>
    /// Request to Evaluate an image. Evaluate Image determines how an image
    /// stands against a set of policies, including adult content.
    /// </summary>
    public class EvaluateImageRequest : BaseImageRequest
    {
        public EvaluateImageRequest(ImageModeratableContent content)
            : base(content)
        {
            this.Metadata = new List<KeyValue>();
        }

        public List<KeyValue> Metadata { get; set; }
    }
}
