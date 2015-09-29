// -----------------------------------------------------------------------
//  <copyright file="EvaluateImageResult.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Response from evaluating an image. The response contains the following information
    /// 1. Score: Returned within the AdvancedInfo, the score contains the actual score from
    /// the adult content classifier. 
    /// 2. Result: Result contains the result of evaluation. 
    /// </summary>
    public class EvaluateImageResult : BaseModeratorResult
    {
        /// <summary>
        /// Array of name value pairs wit specific information about evaluate
        /// </summary>
        public AdvancedInfo[] AdvancedInfo;

        /// <summary>
        /// Image adult classification score
        /// </summary>
        public double AdultClassificationScore;

        /// <summary>
        /// Image is in adult category
        /// </summary>
        public bool IsImageAdultClassified;

        /// <summary>
        /// Image is in racism category
        /// </summary>
        public bool IsImageRacyClassified;

        /// <summary>
        /// Image racy classification score
        /// </summary>
        public double RacyClassificationScore;

    }
}
