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
        public TextModeratableContent(string text, string englishTranslation = null)
        {
            this.ContentAsString = text;
            this.DataRepresentationType = DataRepresentationType.Inline;
            this.EnglishTranslation = englishTranslation;
        }

        /// <summary>
        /// How text is represented. 
        /// </summary>
        private DataRepresentationType DataRepresentationType { get; set; }

        /// <summary>
        /// Data representation of text
        /// </summary>
        public string DataRepresentation { get { return this.DataRepresentationType.ToString(); } }

        /// <summary>
        /// Text content
        /// </summary>
        public string ContentAsString { get; set; }

        /// <summary>
        /// Text representation
        /// </summary>
        //public string DataRepresentation => this.DataRepresentationType.ToString();
        public string DataReperesentation {
            get
            {
                return this.DataRepresentationType.ToString();
            } 
        }

        /// <summary>
        /// English translation
        /// </summary>
        public string EnglishTranslation { get; private set; }
    }
}
