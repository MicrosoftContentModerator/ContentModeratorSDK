// -----------------------------------------------------------------------
//  <copyright file="ImageModeratableContent.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Image
{
    using System.IO;

    /// <summary>
    /// Class representing image content that may be moderated
    /// </summary>
    public class ImageModeratableContent : IModeratableContent
    {
        private DataRepresentationType DataRepresentationType { get; }

        /// <summary>
        /// Content of image as string
        /// </summary>
        public string ContentAsString { get; }

        public BinaryContent BinaryContent { get; }

        public bool ImproveQualityForMatch { get; }
        /// <summary>
        /// Constructor, taking as input a url
        /// </summary>
        /// <param name="url">Url where image is located</param>
        public ImageModeratableContent(string url, bool improveQualityForMatch = false)
        {
            this.ContentAsString = url;
            this.DataRepresentationType = DataRepresentationType.Url;
            this.ImproveQualityForMatch = improveQualityForMatch;

        }

        public ImageModeratableContent(BinaryContent binaryContent, bool improveQualityForMatch = false)
        {
            this.BinaryContent = binaryContent;
            this.ImproveQualityForMatch = improveQualityForMatch;
        }

        /// <summary>
        /// Data representation of image
        /// </susmmary>
        public string DataRepresentation => this.DataRepresentationType.ToString();
    }

    /// <summary>
    /// Image binary content
    /// </summary>
    public class BinaryContent
    {
        public BinaryContent(Stream stream, string contentType)
        {
            this.Stream = stream;
            this.ContentType = contentType;
        }

        /// <summary>
        /// Image Stream
        /// </summary>
        public Stream Stream;

        /// <summary>
        /// Image Content Type
        /// </summary>
        public string ContentType;
    }
}
