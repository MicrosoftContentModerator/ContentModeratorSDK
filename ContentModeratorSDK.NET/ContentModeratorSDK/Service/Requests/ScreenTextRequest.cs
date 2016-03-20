// -----------------------------------------------------------------------
//  <copyright file="ScreenTextRequest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Requests
{
    using ContentModeratorSDK.Text;

    public class ScreenTextRequest : BaseTextRequest
    {
        /// <summary>
        /// Request to screen text, determining if the content violates any policy (adult content, etc...)
        /// </summary>
        /// <param name="content"></param>
        public ScreenTextRequest(TextModeratableContent content)
            : base(content)
        {
            this.Metadata = new string[] { };            
        }

        public string[] Metadata { get; private set; }
    }
}
