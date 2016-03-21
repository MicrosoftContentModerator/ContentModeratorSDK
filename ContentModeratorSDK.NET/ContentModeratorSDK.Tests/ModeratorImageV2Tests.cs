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
    public class ModeratorImageV2Tests
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
              
                ImageServicePathV2 = ConfigurationManager.AppSettings["ImageServicePathV2"],                                
                ImageServiceCustomListPathV2 = ConfigurationManager.AppSettings["ImageServiceCustomListPathV2"],               
                ImageCachingPath = ConfigurationManager.AppSettings["ImageCachingPath"],


                ImageServiceKey = ConfigurationManager.AppSettings["ImageServiceKey"],
                ImageServiceCustomListKey = ConfigurationManager.AppSettings["ImageServiceCustomListKey"]
            };

            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

        }

        /// <summary>
        /// Evaluate an image based on a provided url. This method returns a single valuation score, 
        /// as well as a flag indicating whether evaluation was successful.
        /// </summary>
        [TestMethod]
        [TestCategory("Image.Evaluate")]
        public void EvaluateImageUrlTest()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

            ImageModeratableContent imageContent = new ImageModeratableContent(TestImageUrl);
            var moderateResult = moderatorService.EvaluateImageAsync(imageContent);
            var actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsTrue(actualResult.AdultClassificationScore > 0, "Expected higher than 0 Adult Classification Score value for test image, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsTrue(actualResult.RacyClassificationScore > 0, "Expected higher than 0 Racy Classification Score value for test image, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsTrue(!actualResult.IsImageAdultClassified, "Image should not have been classified as Adult, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsTrue(!actualResult.IsImageRacyClassified, "Image should not have been classified as Racy, Response: {0}", JsonConvert.SerializeObject(actualResult));
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
                IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

                ImageModeratableContent imageContent = new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var moderateResult = moderatorService.EvaluateImageAsync(imageContent);
                var actualResult = moderateResult.Result;
                Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));                
                Assert.IsTrue(actualResult.AdultClassificationScore > 0, "Expected higher than 0 Adult Classification Score value for test image, Response: {0}", JsonConvert.SerializeObject(actualResult));
                Assert.IsTrue(actualResult.RacyClassificationScore > 0, "Expected higher than 0 Racy Classification Score value for test image, Response: {0}", JsonConvert.SerializeObject(actualResult));
                Assert.IsTrue(!actualResult.IsImageAdultClassified, "Image should not have been classified as Adult, Response: {0}", JsonConvert.SerializeObject(actualResult));
                Assert.IsTrue(!actualResult.IsImageRacyClassified, "Image should not have been classified as Racy, Response: {0}", JsonConvert.SerializeObject(actualResult));
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
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

            ImageModeratableContent imageContent = new ImageModeratableContent(TestImageUrl);
            var moderateResult = moderatorService.EvaluateImageAsync(imageContent);
            var actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsTrue(actualResult.AdvancedInfo != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));

            Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score, Response: {0}", JsonConvert.SerializeObject(actualResult));
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
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            Service.Results.EvaluateImageResult actualResult = EvaluateImageContent(moderatorService);
            Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));

            Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score, Response: {0}", JsonConvert.SerializeObject(actualResult));
        }

        private static Service.Results.EvaluateImageResult EvaluateImageContent(IModeratorServiceV2 moderatorService, bool cacheImage = false)
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var moderateResult = moderatorService.EvaluateImageAsync(imageContent, cacheImage);
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
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            Service.Results.EvaluateImageResult actualResult = EvaluateImageContent(moderatorService, true);
            Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));

            Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsNotNull(actualResult.CacheID);
        }

        [TestMethod]
        [TestCategory("Image.Evaluate")]
        [TestCategory("Image.Cache")]
        public void EvaluateImageInCache()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            var actualResult = EvaluateImageContent(moderatorService, true);

            var moderateResult = moderatorService.EvaluateImageInCache(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));

            Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score, Response: {0}", JsonConvert.SerializeObject(actualResult));
        }

        [TestMethod]
        [TestCategory("Image.Cache")]
        public void CacheImage()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            var actualResult = CacheImageContent(moderatorService);
            Assert.AreNotEqual(actualResult.CacheID, "");

        }

        [TestMethod]
        [TestCategory("Image.Cache")]
        public void UnCacheImage()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            var actualResult = CacheImageContent(moderatorService);
            var moderateResult = moderatorService.UnCacheImageContent(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult == null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
            //Assert.IsTrue(actualResult != null, "Expected valid result");
            //Assert.AreEqual(actualResult.Status.Code, "204", "Expected status code");
        }

        private static Service.Results.BaseModeratorResult CacheImageContent(IModeratorServiceV2 moderatorService)
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
        [TestCategory("Image.V2")]
        public void AddImageV2Test()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

            // Add Image (with labels)
            // See label details in the response documentation: https://developer.microsoftmoderator.com/docs/services/54f7932727037412a0cda396/operations/54f793272703740c70627a24
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var addResponse = moderatorService.ImageAddAsync(imageContent, TestTags, TestLabel);
                var addResult = addResponse.Result;
                Assert.IsTrue(addResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(addResult));
                Assert.IsTrue(string.IsNullOrWhiteSpace(addResult.ImageId)
                    || string.Compare(addResult.Status.Description, "Error occurred while processing request :: Failure Adding a valid image  :: Image already exists") == 0,
                    "Image Id can be null only if the Image already exists, Response: {0}", JsonConvert.SerializeObject(addResult));



                // Refresh index
                var refreshResponse = moderatorService.RefreshImageIndexAsync();
                var refreshResult = refreshResponse.Result;
                Assert.IsTrue(refreshResult.IsUpdateSuccess, "Expected update Success on refresh");
            }

            //Wait for Index to be Ready
            while (true)
            {
                var res = moderatorService.CheckImageIndexStatusAsync();
                var response = res.Result;
                if (response.IsSuccessStatusCode && response.ReasonPhrase == "The index is ready for matching.")
                {
                    break;

                }
                else { Thread.Sleep(5000); }
            }


            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));

                // Match
                var matchResponse = moderatorService.MatchImageAsync(imageContent, true);
                var matchResult = matchResponse.Result;
                Assert.IsTrue(matchResult.IsMatch, "Expected match, Response: {0}", JsonConvert.SerializeObject(matchResult));
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
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            Service.Results.DetectFaceResult extractResult = DetectFaceContent(moderatorService, true);

            Assert.IsTrue(extractResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(extractResult));
            Assert.IsTrue(extractResult.Result, "Result is NULL, Response: {0}", JsonConvert.SerializeObject(extractResult));
            Assert.IsNotNull(extractResult.Faces, "Faces is NULL, Response: {0}", JsonConvert.SerializeObject(extractResult));
        }

        [TestMethod]
        [TestCategory("Image.Face")]
        public void DetectFaceInCache()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            var actualResult = DetectFaceContent(moderatorService, true);

            var moderateResult = moderatorService.DetectFaceInCache(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsTrue(actualResult.Result, "Result is NULL, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsNotNull(actualResult.Faces, "Faces is NULL, Response: {0}", JsonConvert.SerializeObject(actualResult));
        }

        [TestMethod]
        [TestCategory("Image.HashIndex")]
        public void CheckHashIndexStatus()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            var actualResult = moderatorService.CheckImageIndexStatusAsync();
            var res = actualResult.Result;
                        
            Assert.IsTrue(res != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.IsTrue(res.StatusCode == System.Net.HttpStatusCode.OK, "Invalid Http Status Code, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.AreEqual("The index is ready for matching.", res.ReasonPhrase, "Response: {0}", JsonConvert.SerializeObject(actualResult));
        }

        private static Service.Results.DetectFaceResult DetectFaceContent(IModeratorServiceV2 moderatorService, bool cacheImage = false)
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
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));

                // extract
                var extractResponse = moderatorService.ExtractTextAsync(imageContent, "eng");
                var extractResult = extractResponse.Result;

                Assert.IsTrue(extractResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(extractResult));
                Assert.IsTrue(extractResult.Text == "Windows10 \r\nSatya Nadella \r\n", "Extracted text is not as expected, Response: {0}", JsonConvert.SerializeObject(extractResult));

                
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
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            Service.Results.ExtractTextResult extractResult = ExtractTextContent(moderatorService, true);

            Assert.IsTrue(extractResult != null, "Expected valid result");
            Assert.IsTrue(extractResult.Candidates != null, "Expected valid result", "Response: {0}", JsonConvert.SerializeObject(extractResult));
            //Assert.AreEqual("THIS IS A \r\nSIMPLE TEST \r\n", extractResult.Text, "Text message was unexpected");
            Assert.AreEqual("Windows10 \r\nSatya Nadella \r\n", extractResult.Text, "Text message was unexpected, Response: {0}", JsonConvert.SerializeObject(extractResult));

        }

        [TestMethod]
        [TestCategory("Image.OCR")]
        public void ExtractTextInCache()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            var actualResult = ExtractTextContent(moderatorService, true);

            var moderateResult = moderatorService.ExtractTextInCache(actualResult.CacheID);
            actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));

            Assert.IsTrue(actualResult.Candidates != null, "Expected valid result, Response: {0}", JsonConvert.SerializeObject(actualResult));
            Assert.AreEqual("Windows10 \r\nSatya Nadella \r\n", actualResult.Text, "Text message was unexpected, Response: {0}", JsonConvert.SerializeObject(actualResult));
        }
        private static Service.Results.ExtractTextResult ExtractTextContent(IModeratorServiceV2 moderatorService, bool cacheImage = false)
        {
            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var extractResponse = moderatorService.ExtractTextAsync(imageContent, "eng", true);
                return extractResponse.Result;

            }
        }

        //PDNA methods

    }
}
