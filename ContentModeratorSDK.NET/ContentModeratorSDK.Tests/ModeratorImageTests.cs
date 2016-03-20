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

    /// <summary>
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
                ImageServicePathV2 = ConfigurationManager.AppSettings["ImageServicePathV2"],                

                // Input your keys after signing up for content moderator below
                // Visit the API manager portal to get keys:
                // https://developer.microsoftmoderator.com/docs/services?ref=mktg
                ImageServiceKey = ConfigurationManager.AppSettings["ImageServiceKey"],
                ImageServiceCustomListKey = ConfigurationManager.AppSettings["ImageServiceCustomListKey"],                
                ImageServiceCustomListPathV2 = ConfigurationManager.AppSettings["ImageServiceCustomListPathV2"],               
                ImageCachingPath = ConfigurationManager.AppSettings["ImageCachingPath"],
                
            };
            
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
            Assert.IsTrue(actualResult != null, "Expected valid result");
            Assert.IsTrue(actualResult.AdvancedInfo != null, "Expected valid result");

            var score = actualResult.AdvancedInfo.First(x => string.Equals(x.Key, "score", StringComparison.OrdinalIgnoreCase));
            Assert.AreNotEqual("0.000", score.Value, "score value");
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
                Assert.IsTrue(actualResult != null, "Expected valid result");
                Assert.IsTrue(actualResult.AdvancedInfo != null, "Expected valid result");

                var score =
                    actualResult.AdvancedInfo.First(
                        x => string.Equals(x.Key, "score", StringComparison.OrdinalIgnoreCase));
                double scoreValue = double.Parse(score.Value);
                Assert.IsTrue(scoreValue > 0, "Expected higher than 0 score value for test image");
            }
        }

        /// <summary>
        /// V2 version of the Evaluate an image based on a provided url. This Api will return
        /// 2 scores. An AdultClassificationScore, which indicates the likelihood an image is adult,
        /// and a RacyClassificationScore which indicates the likelihood an image is racy. A boolean 
        /// result is returned as well for both scores, indicating the server resolution on whether the image
        /// is racy and/or adult.
        /// </summary>
        [TestMethod]
        [TestCategory("Image.Evaluate")]
        public void EvaluateImageUrlV2Test()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            ImageModeratableContent imageContent = new ImageModeratableContent(TestImageUrl);
            var moderateResult = moderatorService.EvaluateImageWithMultipleRatingsAsync(imageContent);
            var actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result");
            Assert.IsTrue(actualResult.AdvancedInfo != null, "Expected valid result");

            Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score");
            Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score");
        }

        /// <summary>
        /// V2 version of the Evaluate an image based on provided content. This Api will return
        /// 2 scores. An AdultClassificationScore, which indicates the likelihood an image is adult,
        /// and a RacyClassificationScore which indicates the likelihood an image is racy. A boolean 
        /// result is returned as well for both scores, indicating the server resolution on whether the image
        /// is racy and/or adult.
        /// </summary>
        [TestMethod]
        [TestCategory("Image.Evaluate")]
        [TestCategory("Image.V2")]
        public void EvaluateImageContentV2Test()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            Service.Results.EvaluateImageResult actualResult = EvaluateImageContent(moderatorService);
            Assert.IsTrue(actualResult != null, "Expected valid result");

            Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score");
            Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score");
        }

        private static Service.Results.EvaluateImageResult EvaluateImageContent(IModeratorService moderatorService, bool cacheImage = false)
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var moderateResult = moderatorService.EvaluateImageWithMultipleRatingsAsync(imageContent, cacheImage);
                return moderateResult.Result;
                
            }
        }

        /// <summary>
        /// V2 version of the Evaluate an image based on provided content. This Api will return
        /// 2 scores. An AdultClassificationScore, which indicates the likelihood an image is adult,
        /// and a RacyClassificationScore which indicates the likelihood an image is racy. A boolean 
        /// result is returned as well for both scores, indicating the server resolution on whether the image
        /// is racy and/or adult.
        /// </summary>
        [TestMethod]
        [TestCategory("Image.Evaluate")]
        [TestCategory("Image.Cache")]
        [TestCategory("Image.V2")]
        public void EvaluateImageContentV2AndCacheTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            Service.Results.EvaluateImageResult actualResult = EvaluateImageContent(moderatorService, true);
            Assert.IsTrue(actualResult != null, "Expected valid result");

            Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score");
            Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score");
            Assert.IsNotNull(actualResult.CacheID);
        }

        [TestMethod]
        [TestCategory("Image.Evaluate")]
        [TestCategory("Image.Cache")]
        public void EvaluateImageInCache()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            var actualResult = EvaluateImageContent(moderatorService, true);

            var moderateResult = moderatorService.EvaluateImageInCache(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result");

            Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score");
            Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score");
        }

        [TestMethod]
        [TestCategory("Image.Cache")]
        public void CacheImage()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            var actualResult = CacheImageContent(moderatorService);
            Assert.AreNotEqual(actualResult.CacheID, "");

        }

        [TestMethod]
        [TestCategory("Image.Cache")]
        public void UnCacheImage()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            var actualResult = CacheImageContent(moderatorService);
            var moderateResult = moderatorService.UnCacheImageContent(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult == null, "Expected valid result");
            //Assert.IsTrue(actualResult != null, "Expected valid result");
            //Assert.AreEqual(actualResult.Status.Code, "204", "Expected status code");
        }

        private static Service.Results.BaseModeratorResult CacheImageContent(IModeratorService moderatorService)
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(TestImageUrl);//(TestCacheImage);
                var moderateResult = moderatorService.CacheImageContent(imageContent);
                return moderateResult.Result;
                //var actualResult = moderateResult.Result;
                //Assert.AreNotEqual(actualResult.CacheID, "");
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
                Assert.IsTrue(addResult != null, "Expected valid result");
                Assert.IsTrue(string.IsNullOrWhiteSpace(addResult.ImageId) 
                    || string.Compare(addResult.Status.Description, "Error occurred while processing request :: Failure Adding a valid image  :: Image already exists") == 0,
                    "Image Id can be null only if the Image already exists");

                

                // Refresh index
                var refreshResponse = moderatorService.RefreshImageIndexAsync();
                var refreshResult = refreshResponse.Result;
                Assert.IsTrue(refreshResult.IsUpdateSuccess, "Expected update Success on refresh");
            }

            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));

                // Match
                var matchResponse = moderatorService.MatchImageAsync(imageContent);
                var matchResult = matchResponse.Result;
                Assert.IsTrue(matchResult.IsMatch, "Expected match");
            }
        }

        /// <summary>
        /// Add Image to image list, then verify it is matched after it was added.
        /// </summary>
        [TestMethod]
        [TestCategory("Image.CustomList")]
        [TestCategory("Image.V2")]
        public void AddImageV2Test()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            // Add Image (with labels)
            // See label details in the response documentation: https://developer.microsoftmoderator.com/docs/services/54f7932727037412a0cda396/operations/54f793272703740c70627a24
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var addResponse = moderatorService.ImageAddAsyncV2(imageContent, TestTags, TestLabel);
                var addResult = addResponse.Result;
                Assert.IsTrue(addResult != null, "Expected valid result");
                Assert.IsTrue(!string.IsNullOrWhiteSpace(addResult.ImageId), "Image ID came back as NULL after adding");


                // Refresh index
                var refreshResponse = moderatorService.RefreshImageIndexV2Async();
                var refreshResult = refreshResponse.Result;
                Assert.IsTrue(refreshResult.IsUpdateSuccess, "Expected update Success on refresh");
            }

            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));

                // Match
                var matchResponse = moderatorService.MatchImageAsyncV2(imageContent, true);
                var matchResult = matchResponse.Result;
                Assert.IsTrue(matchResult.IsMatch, "Expected match");
            }
        }

        

        /// <summary>
        /// Detect faces from an image using OCR
        /// </summary>
        [TestMethod]
        [TestCategory("Image.Face")]
        [TestCategory("Image.V2")]
        public void DetectFaceV2AndCacheTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            Service.Results.DetectFaceResult extractResult = DetectFaceContent(moderatorService, true);

            Assert.IsTrue(extractResult != null, "Expected valid result");
            Assert.IsTrue(extractResult.Result, "Expected valid result");
            Assert.IsNotNull(extractResult.Faces, "Expected valid result");
        }

        [TestMethod]
        [TestCategory("Image.Face")]
        public void DetectFaceInCache()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            var actualResult = DetectFaceContent(moderatorService, true);

            var moderateResult = moderatorService.DetectFaceInCache(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result");
            Assert.IsTrue(actualResult.Result, "Expected valid result");
            Assert.IsNotNull(actualResult.Faces, "Expected valid result");
        }

        [TestMethod]
        [TestCategory("Image.HashIndex")]
        public void CheckHashIndexStatus()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            var actualResult = DetectFaceContent(moderatorService, true);

            var moderateResult = moderatorService.DetectFaceInCache(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result");
            Assert.IsTrue(actualResult.Result, "Expected valid result");
            Assert.IsNotNull(actualResult.Faces, "Expected valid result");
        }

        private static Service.Results.DetectFaceResult DetectFaceContent(IModeratorService moderatorService, bool cacheImage = false)
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var extractResponse = moderatorService.DetectFaceAsync(imageContent, true);
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

                Assert.IsTrue(extractResult != null, "Expected valid result");
                Assert.IsTrue(extractResult.AdvancedInfo != null, "Expected valid result");

                var text =
                    extractResult.AdvancedInfo.First(
                        x => string.Equals(x.Key, "Text", StringComparison.OrdinalIgnoreCase));

                Assert.AreEqual("Windows10 \r\nSatya Nadella \r\n", text.Value, "Text message was unexpected");
            }
        }

        /// <summary>
        /// Extract the text from an image using OCR
        /// </summary>
        [TestMethod]
        [TestCategory("Image.OCR")]
        [TestCategory("Image.V2")]
        public void ExtractTextV2AndCacheTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            Service.Results.ExtractTextResult extractResult = ExtractTextContent(moderatorService, true);

            Assert.IsTrue(extractResult != null, "Expected valid result");
            Assert.IsTrue(extractResult.Candidates != null, "Expected valid result");
            //Assert.AreEqual("THIS IS A \r\nSIMPLE TEST \r\n", extractResult.Text, "Text message was unexpected");
            Assert.AreEqual("Windows10 \r\nSatya Nadella \r\n", extractResult.Text, "Text message was unexpected");

        }

        [TestMethod]
        [TestCategory("Image.OCR")]
        public void ExtractTextInCache()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);
            var actualResult = ExtractTextContent(moderatorService, true);

            var moderateResult = moderatorService.ExtractTextInCache(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result");

            Assert.IsTrue(actualResult.Candidates != null, "Expected valid result");
            Assert.AreEqual("Windows10 \r\nSatya Nadella \r\n", actualResult.Text, "Text message was unexpected");
        }
        private static Service.Results.ExtractTextResult ExtractTextContent(IModeratorService moderatorService, bool cacheImage = false)
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var extractResponse = moderatorService.ExtractTextAsyncV2(imageContent, "eng", true);
                return extractResponse.Result;

            }
        }

        //PDNA methods

    }
}
