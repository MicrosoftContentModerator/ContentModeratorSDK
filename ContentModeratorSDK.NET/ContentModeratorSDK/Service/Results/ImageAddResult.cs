// -----------------------------------------------------------------------
//  <copyright file="ImageAddResult.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Result from Image Add Operation.
    /// The response will contain the following details:
    /// Source: Unique Id for the Content source for the user, returned on the Additional Info array
    /// ImageId: Unique Id of the image in the system.
    /// </summary>
    public class ImageAddResult : BaseModeratorResult
    {
        /// <summary>
        /// Detailed information from image add action
        /// </summary>
        public AdvancedInfo[] AdditionalInfo;

        /// <summary>
        /// Id of added image
        /// </summary>
        public string ImageId;
    }
}
