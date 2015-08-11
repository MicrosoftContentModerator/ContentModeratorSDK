// -----------------------------------------------------------------------
//  <copyright file="IModeratorResult.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK
{
    using ContentModeratorSDK.Service.Results;

    /// <summary>
    /// Response from Moderator
    /// </summary>
    public interface IModeratorResult
    {
        /// <summary>
        /// Status containing general information from response
        /// </summary>
        Status Status { get; }

        /// <summary>
        /// TrackingId from response
        /// </summary>
        string TrackingId { get; }
    }
}
