// -----------------------------------------------------------------------
//  <copyright file="AddTermResult.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Response from Add Term operation. After adding a term,
    /// The advanced info array will contain details related to the term.
    /// The ContentId is a unique Id of the term after being added into the system.
    /// </summary>
    public class AddTermResult : BaseModeratorResult
    {
        /// <summary>
        /// Details on Response
        /// </summary>
        public AdvancedInfo[] AdditionalInfo;

        /// <summary>
        /// Unique Id of the added content
        /// </summary>
        public string ContentId;
    }
}
