// -----------------------------------------------------------------------
//  <copyright file="ScreenTextResult.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Result from screening a text, containing details regarding the Match
    /// </summary>
    public class ScreenTextResult : BaseModeratorResult
    {
        public string ContentId;
        public bool IsMatch;

        /// <summary>
        /// The match details contain information about the match in the AdvancedInfo. Including:
        /// MatchStartIndex: Where in the text did the match start
        /// MatchEndIndex: Where in the text did the match end
        /// </summary>
        public MatchDetails MatchDetails;
    }
}
