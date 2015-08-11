// -----------------------------------------------------------------------
//  <copyright file="IModeratableContent.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK
{
    /// <summary>
    /// Interface for moderatable content
    /// </summary>
    public interface IModeratableContent
    {
        /// <summary>
        /// Data Representation
        /// </summary>
        string DataRepresentation { get; }

        /// <summary>
        /// Actual content in string format
        /// </summary>
        string ContentAsString { get; }
    }
}
