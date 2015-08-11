// -----------------------------------------------------------------------
//  <copyright file="ImageRefreshIndexResult.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Result from refreshing an image index.
    /// The result will contain:
    /// IsUpdateSuccess: Flag indicating if the update was successful. If not, an error will be returned in this field.
    /// ContentSourceId: Id of the content source refreshed.
    /// In the status field, additional details for the refresh operation will be returned as well.
    /// </summary>
    public class ImageRefreshIndexResult : BaseModeratorResult
    {
        /// <summary>
        /// Advanced Information from the request
        /// </summary>
        public AdvancedInfo[] AdvancedInfo;

        /// <summary>
        /// Boolean indicating if the update was succssful
        /// </summary>
        public bool IsUpdateSuccess;

        /// <summary>
        /// Content Source refreshed
        /// </summary>
        public string ContentSourceId;
    }
}
