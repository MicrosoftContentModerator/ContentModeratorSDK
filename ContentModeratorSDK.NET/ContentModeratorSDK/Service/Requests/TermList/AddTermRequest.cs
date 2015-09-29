// -----------------------------------------------------------------------
//  <copyright file="AddTermRequest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service.Requests.TermList
{
    using ContentModeratorSDK.Text;

    /// <summary>
    /// Request to add a Term into the service
    /// </summary>
    public class AddTermRequest : BaseModeratorRequest
    {
        public Content Content;

        public AddTermRequest(TextModeratableContent content)
        {
            this.Content = new Content()
            {
                DataRepresentation = content.DataRepresentation,
                Value = content.ContentAsString,
                EnglishTranslation = content.EnglishTranslation,
            };
        }
    }

    /// <summary>
    /// Term content
    /// </summary>
    public class Content
    {
        public string DataRepresentation;
        public string EnglishTranslation;
        public string Labels;
        public string Value;
    }
}
