// -----------------------------------------------------------------------
//  <copyright file="BaseModeratorResult.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Base class from a Moderator Api result
    /// </summary>
    public class BaseModeratorResult : IModeratorResult
    {
        /// <summary>
        /// Status of the result
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Tracking Id
        /// </summary>
        public string TrackingId { get; set; }

        /// <summary>
        /// Cache Id
        /// </summary>
        public string CacheID { get; set; }
    }

    /// <summary>
    /// Status from a response
    /// </summary>
    public class Status
    {
        /// <summary>
        /// Status code
        /// </summary>
        public string Code;

        /// <summary>
        /// Text describing the response
        /// </summary>
        public string Description;

        /// <summary>
        /// Observed error
        /// </summary>
        public string Exception;
    }

    /// <summary>
    /// Name value pair with detailed information on the response
    /// </summary>
    public class AdvancedInfo
    {
        public string Key;
        public string Value;
    }
}
