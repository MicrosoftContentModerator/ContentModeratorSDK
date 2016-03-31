using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContentModeratorSDK.Service;
using System.Configuration;
using ContentModeratorSDK.Image;

namespace ContentModeratorSDK.Tests
{
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    [TestClass]
    public class PDNAServiceTests
    {
        private const string TestImageUrl = "https://cdn.schedulicity.com/usercontent/b9de6e06-e954-4169-ac44-57fa1c3b4245.jpg";
        public const string noMatchImageContent = @"Content\sample.jpg";
        public const string matchImageContent = @"Content\pdnasample.jpg";

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
        public void ValidateImageUrl()
        {
            IPDNAService pdnaService = new PDNAService(this.pdnaServiceOptions);
            Service.Results.MatchImageResult extractResult = ValidateImageContent(pdnaService, false);

            Assert.IsTrue(extractResult != null, "Expected valid result");
            Assert.IsTrue(extractResult.IsMatch, "Expected valid result");
            Assert.IsNotNull(extractResult.MatchDetails, "Expected valid result");
        }

        /// <summary>
        /// Simply calling the PDNA Cloud Service with the Test Image without Caching
        /// </summary>
        [TestMethod]
        [TestCategory("PDNASvc")]
        public void ValidateImageBinaryContentMatch()
        {
            using (Stream stream = new FileStream(matchImageContent, FileMode.Open, FileAccess.Read))
            {
                IPDNAService pdnaService = new PDNAService(this.pdnaServiceOptions);

                ImageModeratableContent imageContent = new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var moderateResult = pdnaService.ValidateImageAsync(imageContent,false);
                var actualResult = moderateResult.Result;
                Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
                Assert.IsTrue(actualResult.MatchDetails.MatchFlags.Count() >0, "Expected Match Count to be greater than 0, Response: {0}", JsonConvert.SerializeObject(actualResult));

            }
        }

        /// <summary>
        /// Simply calling the PDNA Cloud Service with the Test Image without Caching
        /// </summary>
        [TestMethod]
        [TestCategory("PDNASvc")]
        public void ValidateImageBinaryContentNoMatch()
        {
            using (Stream stream = new FileStream(noMatchImageContent, FileMode.Open, FileAccess.Read))
            {
                IPDNAService pdnaService = new PDNAService(this.pdnaServiceOptions);

                ImageModeratableContent imageContent = new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var moderateResult = pdnaService.ValidateImageAsync(imageContent, false);
                var actualResult = moderateResult.Result;
                Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
                Assert.IsTrue(actualResult.MatchDetails.MatchFlags.Count() == 0, "No Match was expected for this image, Response: {0}", JsonConvert.SerializeObject(actualResult));

            }
        }



        /// <summary>
        /// First call the Validate method with Image Caching set to true and get back the CacheId
        /// Next call the Validate method again sending in the Cache Id that was generated in the previous call
        /// </summary>
        [TestMethod]
        [TestCategory("PDNASvc")]
        public void ValidateImageInCache()
        {
            IPDNAService pdnaService = new PDNAService(this.pdnaServiceOptions);
            var actualResult = ValidateImageContent(pdnaService, true);

            Assert.IsTrue(actualResult != null, "Expected valid result");
            Assert.IsTrue(actualResult.IsMatch, "Expected valid result");
            Assert.IsNotNull(actualResult.CacheID, "Cache ID should not be null");


            var pdnaResult = pdnaService.ValidateImageInCache(actualResult.CacheID);
            var matchRes = pdnaResult.Result;
            Assert.IsTrue(matchRes != null, "Expected valid result");
            Assert.IsTrue(matchRes.IsMatch, "Expected valid result");
            Assert.IsNotNull(matchRes.MatchDetails, "Expected valid result");
        }

        private static Service.Results.MatchImageResult ValidateImageContent(IPDNAService pdnaService, bool cacheImage = false)
        {
            ImageModeratableContent imageContent =
                new ImageModeratableContent("https://pdnasampleimages.blob.core.windows.net/matchedimages/img_130.jpg");
            var extractResponse = pdnaService.ValidateImageAsync(imageContent, cacheImage);
            return extractResponse.Result;

        }


        
    }
}
