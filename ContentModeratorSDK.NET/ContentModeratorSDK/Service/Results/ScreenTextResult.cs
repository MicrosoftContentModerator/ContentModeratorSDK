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
        public string Language;
        public string NormalizedText;

        public MatchTerm[] Terms;

        public MatchUrl[] Urls;

        public string ContentId;
        public bool IsMatch;
        
        public MatchDetails MatchDetails;
    }

    public class MatchTerm
    {
        public string Term;
        public int Index;
    }

    public class MatchUrl
    {
        public MatchUrlCategories categories;

        public string URL;
    }

    public class MatchUrlCategories
    {
        public int Adult;
        public int Malware;
        public int Phishing;
    }
}
