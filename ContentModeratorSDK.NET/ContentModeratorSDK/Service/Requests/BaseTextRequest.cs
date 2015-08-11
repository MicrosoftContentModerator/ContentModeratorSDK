// -----------------------------------------------------------------------
//  <copyright file="BaseTextRequest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Requests
{
    using System;
    using ContentModeratorSDK.Text;

    /// <summary>
    /// Base text request for the content moderator service
    /// </summary>
    public class BaseTextRequest : BaseModeratorRequest
    {
        public BaseTextRequest(TextModeratableContent textContent)
        {
            if (textContent == null)
            {
                throw new ArgumentNullException(nameof(textContent), "Input text content is null");
            }

            this.DataRepresentation = textContent.DataRepresentation;
            this.Value = textContent.ContentAsString;
        }

        /// <summary>
        /// Text data representation
        /// </summary>
        public string DataRepresentation { private set; get; }

        /// <summary>
        /// Text data content
        /// </summary>
        public string Value { private set; get; }
    }
}
