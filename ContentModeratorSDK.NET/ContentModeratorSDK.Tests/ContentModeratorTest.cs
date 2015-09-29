﻿// -----------------------------------------------------------------------
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
    public class ContentModeratorSDKTest
    {
        private const string TestImageUrl = "https://cdn.schedulicity.com/usercontent/b9de6e06-e954-4169-ac44-57fa1c3b4245.jpg";

        public const string TestImageContent = @"Content\test.jpg";

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
                TextServicePath = ConfigurationManager.AppSettings["TextServicePath"],
                TextServiceCustomListPath = ConfigurationManager.AppSettings["TextServiceCustomListPath"],
                ImageServiceCustomListPath = ConfigurationManager.AppSettings["ImageServiceCustomListPath"],
                ImageServicePathV2 = ConfigurationManager.AppSettings["ImageServicePathV2"],

                // Input your keys after signing up for content moderator below
                // Visit the API manager portal to get keys:
                // https://developer.microsoftmoderator.com/docs/services?ref=mktg
                ImageServiceKey = ConfigurationManager.AppSettings["ImageServiceKey"],
                TextServiceKey = ConfigurationManager.AppSettings["TextServiceKey"],
                TextServiceCustomListKey = ConfigurationManager.AppSettings["TextServiceCustomListKey"],
                ImageServiceCustomListKey = ConfigurationManager.AppSettings["ImageServiceCustomListKey"],
            };
        }

        /// <summary>
        /// Evaluate an image based on a provided url. This method returns a single valuation score, 
        /// as well as a flag indicating whether evaluation was successful.
        /// </summary>
        [TestMethod]
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
        public void EvaluateImageContentV2Test()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            using (Stream stream = new FileStream(TestImageContent, FileMode.Open, FileAccess.Read))
            {
                ImageModeratableContent imageContent =
                    new ImageModeratableContent(new BinaryContent(stream, "image/jpeg"));
                var moderateResult = moderatorService.EvaluateImageWithMultipleRatingsAsync(imageContent);
                var actualResult = moderateResult.Result;
                Assert.IsTrue(actualResult != null, "Expected valid result");

                Assert.AreNotEqual(actualResult.AdultClassificationScore, "0.000", "Adult Score");
                Assert.AreNotEqual(actualResult.RacyClassificationScore, "0.000", "Racy Score");
            }
        }

        /// <summary>
        /// Add Image to image list, then verify it is matched after it was added.
        /// </summary>
        [TestMethod]
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
        /// Extract the text from an image using OCR
        /// </summary>
        [TestMethod]
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

                Assert.AreEqual("THIS IS A \r\nSIMPLE TEST \r\n", text.Value, "Text message was unexpected");
            }
        }

        /// <summary>
        /// Screen text against the default list of terms for english. Validate that the text is matched.
        /// </summary>
        [TestMethod]
        public void ScreenTextTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            // Import the term list. This needs to only be done once before screen
            var taskResult = moderatorService.ImportTermListAsync("eng");
            var actualResult = taskResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result for ImportTermList");

            // Run screen to match, validating match result
            string text = "My evil freaking text!";
            TextModeratableContent textContent = new TextModeratableContent(text);
            var screenResponse = moderatorService.ScreenTextAsync(textContent, "eng");
            var screenResult = screenResponse.Result;
            Assert.IsTrue(screenResult != null, "Expected valid result");
            Assert.IsTrue(screenResult.MatchDetails != null, "Expected valid Match Details");
            Assert.IsTrue(screenResult.MatchDetails.MatchFlags != null, "Expected valid Match Flags");

            var matchFlag = screenResult.MatchDetails.MatchFlags.FirstOrDefault();
            Assert.IsTrue(matchFlag != null, "Expected to see a match flag!");
            Assert.AreEqual("freaking", matchFlag.Source, "Expected term to match");
        }

        /// <summary>
        /// Add a term to the list, and validate we are able to match to it
        /// </summary>
        [TestMethod]
        public void AddTermTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            // We are creating a term "FakeProfanity" in english (thus provide tha same english translation), then matching against it.
            TextModeratableContent textContent = new TextModeratableContent(text:"FakeProfanity", englishTranslation:"FakeProfanity");
            var taskResult = moderatorService.AddTermAsync(textContent, "eng");
            var actualResult = taskResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result for AddTerm");

            var refreshTask = moderatorService.RefreshTextIndexAsync("eng");
            var refreshResult = refreshTask.Result;
            Assert.IsTrue(refreshResult != null, "Expected valid result for RefreshIndex");

            var screenResponse = moderatorService.ScreenTextAsync(new TextModeratableContent("This is a FakeProfanity!"), "eng");
            var screenResult = screenResponse.Result;
            Assert.IsTrue(screenResult.IsMatch, "Expected IsMatch to be true");
            Assert.IsTrue(screenResult != null, "Expected valid result");
            Assert.IsTrue(screenResult.MatchDetails != null, "Expected valid Match Details");
            Assert.IsTrue(screenResult.MatchDetails.MatchFlags != null, "Expected valid Match Flags");

            var deleteTask = moderatorService.RemoveTermAsync(textContent, "eng");
            var deleteResult = deleteTask.Result;
            Assert.IsTrue(deleteResult != null, "Expected valid result for DeleteTerm");
        }

        /// <summary>
        /// Identify the language of an input text
        /// </summary>
        [TestMethod]
        public void IdentifyLanguageTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            TextModeratableContent textContent = new TextModeratableContent("Hola este es un texto en otro idioma");
            var identifyLanguageResponse = moderatorService.IdentifyLanguageAsync(textContent);
            var actualResult = identifyLanguageResponse.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result");
            Assert.AreEqual("spa", actualResult.DetectedLanguage, "Expected valid result");
        }
    }
}
