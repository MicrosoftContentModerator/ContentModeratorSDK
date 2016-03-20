using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContentModeratorSDK.Service;
using System.Configuration;
using ContentModeratorSDK.Image;

namespace ContentModeratorSDK.Tests
{
    [TestClass]
    public class PDNAServiceTests
    {
        private const string TestImageUrl = "https://cdn.schedulicity.com/usercontent/b9de6e06-e954-4169-ac44-57fa1c3b4245.jpg";
        public const string TestImageContent = @"Content\sample.jpg";

        public const string TestTags = "101,102";
        public const string TestLabel = "TestImage";
        
        private PDNAServiceOptions pdnaServiceOptions;

        /// <summary>
        /// Initialize the options for moderator
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
        
            this.pdnaServiceOptions = new PDNAServiceOptions
            {

                HostUrl = ConfigurationManager.AppSettings["PDNAHostUrl"],
                PDNAImageServiceKey = ConfigurationManager.AppSettings["PDNAServiceKey"],
                PDNAImageServicePath = ConfigurationManager.AppSettings["PDNAImageServicePath"],
            };
        }

        
        /// <summary>
        /// Simply calling the PDNA Cloud Service with the Test Image without Caching
        /// </summary>
        [TestMethod]
        [TestCategory("PDNASvc")]
        public void ValidateImageSimple()
        {
            IPDNAService pdnaService = new PDNAService(this.pdnaServiceOptions);
            Service.Results.MatchImageResult extractResult = ValidateImageContent(pdnaService, false);

            Assert.IsTrue(extractResult != null, "Expected valid result");
            Assert.IsTrue(extractResult.IsMatch, "Expected valid result");
            Assert.IsNotNull(extractResult.MatchDetails, "Expected valid result");
        }


        /// <summary>
        /// First call the Validate method with Image Caching set to true and get back the CacheId
        /// Next call the Validate method again sending in the Cache Id that was generated in the previous call
        /// </summary>
        //[TestMethod]
        //[TestCategory("PDNASvc")]
        //public void ValidateImageInCache()
        //{
        //    IPDNAService pdnaService = new PDNAService(this.pdnaServiceOptions);
        //    var actualResult = ValidateImageContent(pdnaService, true);

        //    var pdnaResult = pdnaService.ValidateImageInCache(actualResult.CacheID);
        //    actualResult = pdnaResult.Result;
        //    Assert.IsTrue(actualResult != null, "Expected valid result");
        //    Assert.IsTrue(actualResult.IsMatch, "Expected valid result");
        //    Assert.IsNotNull(actualResult.MatchDetails, "Expected valid result");
        //}

        private static Service.Results.MatchImageResult ValidateImageContent(IPDNAService pdnaService, bool cacheImage = false)
        {
            ImageModeratableContent imageContent =
                new ImageModeratableContent("https://wbintcvsstorage.blob.core.windows.net/matchedimages/img_130.jpg");
            var extractResponse = pdnaService.ValidateImageAsync(imageContent, cacheImage);
            return extractResponse.Result;

        }
    }
}
