// -----------------------------------------------------------------------
//  <copyright file="ContentModeratorTest.cs" company="Microsoft Corporation">
//      Copyright (C) Microsoft Corporation. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace ContentModeratorSDK.Tests
{
    using System;
    using System.IO;
    using System.Linq;
    using ContentModeratorSDK;
    using ContentModeratorSDK.Image;
    using ContentModeratorSDK.Service;
    using ContentModeratorSDK.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Configuration;
    using System.Threading;
    using Newtonsoft.Json;    /// <summary>
                              /// End to end tests for the Content Moderator service
                              /// </summary>
    [TestClass]
    public class ModeratorImageTests
    {
        private const string TestImageUrl = "https://cdn.schedulicity.com/usercontent/b9de6e06-e954-4169-ac44-57fa1c3b4245.jpg";
        public const string TestImageContent = @"Content\sample.jpg";

        public const string TestTags = "101,102";
        public const string TestLabel = "TestImage";
        private ModeratorServiceOptions serviceOptions;        

        /// <summary>
        /// Initialize the options for moderator
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.serviceOptions = new ModeratorServiceOptions()
            {
                HostUrl = ConfigurationManager.AppSettings["HostUrl"],
                ImageServicePath = ConfigurationManager.AppSettings["ImageServicePath"],
                ImageServiceCustomListPath = ConfigurationManager.AppSettings["ImageServiceCustomListPath"],
                ImageServiceKey = ConfigurationManager.AppSettings["ImageServiceKey"],
                ImageServiceCustomListKey = ConfigurationManager.AppSettings["ImageServiceCustomListKey"]
            };

            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

        }

        /// <summary>
        /// Evaluate an image based on a provided url. This method returns a single valuation score, 
        /// as well as a flag indicating whether evaluation was successful.
        /// </summary>
        [TestMethod]
        [TestCategory("Image.Evaluate")]
        public void EvaluateImageUrlTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            ImageModeratableContent imageContent = new ImageModeratableContent(TestImageUrl);
            var moderateResult = moderatorService.EvaluateImageAsync(imageContent);
            var actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsTrue(actualResult.AdvancedInfo != null, "AdvancedInfo is NULL, Response: {0}",  JsonConvert.SerializeObject(actualResult));

            var score = actualResult.AdvancedInfo.First(x => string.Equals(x.Key, "score", StringComparison.OrdinalIgnoreCase));
            Assert.AreNotEqual("0.000", score.Value, "score value, Response: {0}", JsonConvert.SerializeObject(actualResult));
        }

        /// <summary>
        /// Evaluate an image providing binary content inline to the request. This method returns a single valuation score, 
        /// as well as a flag indicating whether evaluation was successful.
        /// </summary>
        [TestMethod]
        [TestCategory("Image.Evaluate")]
        public void EvaluateImageContentTest()
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

                ImageModeratableContent imageContent = new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var moderateResult = moderatorService.EvaluateImageAsync(imageContent);
                var actualResult = moderateResult.Result;
                Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
                Assert.IsTrue(actualResult.AdvancedInfo != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));

                var score =
                    actualResult.AdvancedInfo.First(
                        x => string.Equals(x.Key, "score", StringComparison.OrdinalIgnoreCase));
                double scoreValue = double.Parse(score.Value);
                Assert.IsTrue(scoreValue > 0, "Expected higher than 0 score value for test image, Response: {0}", JsonConvert.SerializeObject(actualResult));
            }
        }

        

        

        private static Service.Results.EvaluateImageResult EvaluateImageContent(IModeratorService moderatorService)
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var moderateResult = moderatorService.EvaluateImageAsync(imageContent);
                return moderateResult.Result;
                
            }
        }
                 
        /// <summary>
        /// Add Image to image list, then verify it is matched after it was added.
        /// </summary>
        [TestMethod]
        [TestCategory("Image.CustomList")]
        public void AddImageTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            // Add Image (with labels)
            // See label details in the response documentation: https://developer.microsoftmoderator.com/docs/services/54f7932727037412a0cda396/operations/54f793272703740c70627a24
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var addResponse = moderatorService.ImageAddAsync(imageContent);
                var addResult = addResponse.Result;
                Assert.IsTrue(addResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(addResult));
                Assert.IsTrue(string.IsNullOrWhiteSpace(addResult.ImageId) 
                    || string.Compare(addResult.Status.Description, "Error occurred while processing request :: Failure Adding a valid image  :: Image already exists") == 0,
                    "Image Id can be null only if the Image already exists, Response: {0}", JsonConvert.SerializeObject(addResult));                

                // Refresh index
                var refreshResponse = moderatorService.RefreshImageIndexAsync();
                var refreshResult = refreshResponse.Result;
                Assert.IsTrue(refreshResult.IsUpdateSuccess, "Expected update Success on refresh, Response: {0}", JsonConvert.SerializeObject(refreshResult));
            }

            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));

                // Match
                var matchResponse = moderatorService.MatchImageAsync(imageContent);
                var matchResult = matchResponse.Result;
                Assert.IsTrue(matchResult.IsMatch, "Expected match, Response: {0}", JsonConvert.SerializeObject(matchResult));
            }
        }

        
        

       

        

        private static Service.Results.DetectFaceResult DetectFaceContent(IModeratorService moderatorService)
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var extractResponse = moderatorService.DetectFaceAsync(imageContent);
                return extractResponse.Result;

            }
        }


        /// <summary>
        /// Extract the text from an image using OCR
        /// </summary>
        [TestMethod]
        [TestCategory("Image.OCR")]
        public void ExtractTextTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));

                // extract
                var extractResponse = moderatorService.ExtractTextAsync(imageContent, "eng");
                var extractResult = extractResponse.Result;

                Assert.IsTrue(extractResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(extractResult));
                Assert.IsTrue(extractResult.AdvancedInfo != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(extractResult));

                var text =
                    extractResult.AdvancedInfo.First(
                        x => string.Equals(x.Key, "Text", StringComparison.OrdinalIgnoreCase));

                Assert.AreEqual("Windows10 \r\nSatya Nadella \r\n", text.Value, "Text message was unexpected, Response: {0}", JsonConvert.SerializeObject(extractResult));
            }
        }
        
    }
}
