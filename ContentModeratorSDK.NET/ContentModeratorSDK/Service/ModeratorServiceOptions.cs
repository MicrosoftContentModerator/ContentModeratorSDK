// -----------------------------------------------------------------------
//  <copyright file="ModeratorServiceOptions.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Service
{
    public class ModeratorServiceOptions
    {
        /// <summary>
        /// Url of host
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// Url Path to image service
        /// </summary>
        public string ImageServicePath { get; set; }

        /// <summary>
        /// Url Path to image service
        /// </summary>
        public string ImageServicePathV2 { get; set; }

        /// <summary>
        /// Url Path to text service
        /// </summary>
        public string TextServicePath { get; set; }

        /// <summary>
        /// Url Path to V2 text service
        /// </summary>
        public string TextServicePathV2 { get; set; }

        /// <summary>
        /// Key for image service
        /// </summary>
        public string ImageServiceKey { get; set; }

        /// <summary>
        /// Key for text service
        /// </summary>
        public string TextServiceKey { get; set; }

        /// <summary>
        /// Key for custom text service
        /// </summary>
        public string TextServiceCustomListKey { get; set; }

        /// <summary>
        /// Key for custom image service
        /// </summary>
        public string ImageServiceCustomListKey { get; set; }       

        /// <summary>
        /// Url Path for custom text list service
        /// </summary>
        public string TextServiceCustomListPath { get; set; }

        /// <summary>
        /// Url Path for custom image list service
        /// </summary>
        public string ImageServiceCustomListPath { get; set; }

        /// <summary>
        /// Url Path for custom image list service V2
        /// </summary>
        public string ImageServiceCustomListPathV2 { get; set; }

        /// <summary>
        /// Url Path for Image caching api
        /// </summary>
        public string ImageCachingPath { get; set; }

        /// <summary>
        /// Key for Image caching API.
        /// </summary>
        public string ImageCachingKey { get; set; }

        /// <summary>
        /// Url Path to PDNA image service
        /// </summary>
        public string PDNAImageServicePath { get; set; }

        /// <summary>
        /// Key for PDNA image service
        /// </summary>
        public string PDNAImageServiceKey { get; set; }

        public string TextContentSourceId { get; set; }
    }
}
