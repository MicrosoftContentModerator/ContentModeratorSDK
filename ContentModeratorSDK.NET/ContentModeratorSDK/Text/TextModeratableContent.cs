// -----------------------------------------------------------------------
//  <copyright file="TextModeratableContent.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Text
{
    /// <summary>
    /// Text content that may be moderated
    /// </summary>
    public class TextModeratableContent : IModeratableContent
    {
        public TextModeratableContent(string text)
        {
            this.ContentAsString = text;
            this.DataRepresentationType = DataRepresentationType.Inline;
        }

        /// <summary>
        /// How text is represented. 
        /// </summary>
        private DataRepresentationType DataRepresentationType { get; }

        /// <summary>
        /// Text content
        /// </summary>
        public string ContentAsString { get; }

        /// <summary>
        /// Text representation
        /// </summary>
        public string DataRepresentation => this.DataRepresentationType.ToString();
    }
}
