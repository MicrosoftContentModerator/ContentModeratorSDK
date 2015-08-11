// -----------------------------------------------------------------------
//  <copyright file="MatchImageResult.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Result from Matching an image.
    /// </summary>
    public class MatchImageResult : BaseModeratorResult
    {
        /// <summary>
        /// Match image content Id
        /// </summary>
        public string ContentId;

        /// <summary>
        /// Boolean indicating whether an image is a match
        /// </summary>
        public bool IsMatch;

        /// <summary>
        /// Match details
        /// </summary>
        public MatchDetails MatchDetails;
    }

    /// <summary>
    /// Specific information about a match
    /// </summary>
    public class MatchDetails
    {
        /// <summary>
        /// Advanced Information from the match
        /// </summary>
        public AdvancedInfo[] AdvancedInfo;

        /// <summary>
        /// The match flags containing details on the match operation
        /// </summary>
        public MatchFlag[] MatchFlags;
    }

    /// <summary>
    /// Information about a match including the source ContentId and the actual score
    /// </summary>
    public class MatchFlag
    {
        /// <summary>
        /// Additional details about match
        /// </summary>
        public AdvancedInfo[] AdvancedInfo;

        /// <summary>
        /// Match score. 1 means exact match.
        /// </summary>
        public double Score;

        /// <summary>
        /// Match source ContentId
        /// </summary>
        public string Source;
    }
}
