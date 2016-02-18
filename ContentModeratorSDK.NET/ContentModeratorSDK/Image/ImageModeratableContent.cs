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
        private DataRepresentationType DataRepresentationType { get; set; }

        /// <summary>
        /// Content of image as string
        /// </summary>
        public string ContentAsString { get; private set; }

        public BinaryContent BinaryContent { get; private set; }

        /// <summary>
        /// Constructor, taking as input a url
        /// </summary>
        /// <param name="url">Url where image is located</param>
        public ImageModeratableContent(string url)
        {
            this.ContentAsString = url;
            this.DataRepresentationType = DataRepresentationType.Url;
        }

        public ImageModeratableContent(BinaryContent binaryContent)
        {
            this.BinaryContent = binaryContent;
        }

        /// <summary>
        /// Data representation of image
        /// </summary>
        public string DataRepresentation { get { return this.DataRepresentationType.ToString(); } }
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
