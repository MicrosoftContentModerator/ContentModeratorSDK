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

    [TestClass]
    public class ContentModeratorSDKTest
    {
        private const string TestImageUrl =
            "http://c.s-microsoft.com/en-us/CMSImages/hero.jpg?version=e5df080c-643c-6e0a-d7ec-ac3e1a093add";

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
                HostUrl = "https://api.microsoftmoderator.com",
                ImageServicePath = "ImageModerator/v1",
                TextServicePath = "TextModerator/v1",
                TextServiceCustomListPath = "Text/Admin",
                ImageServiceCustomListPath = "Image/Validate",

                // Input your keys after signing up for content moderator below
                // Visit the API manager portal to get keys:
                // https://developer.microsoftmoderator.com/docs/services?ref=mktg
                ImageServiceKey = "a4bc9ad8-7517-4934-a903-0b0fad380801",
                TextServiceKey = "708301853fce4454b168f719d0b349af",
                TextServiceCustomListKey = "7c6144a02722463dbe07eceead3d3fdb",
                ImageServiceCustomListKey = "2766c10e-04e5-4139-9aa8-30eb6fd3b499",
            };
        }

        /// <summary>
        /// Evaluate an image based on a provided url
        /// </summary>
        [TestMethod]
        public void EvaluateImageUrlTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            ImageModeratableContent imageContent = new ImageModeratableContent(TestImageUrl);
            var moderateResult = moderatorService.EvaluateImageAsync(imageContent);
            var actualResult = moderateResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result");
            Assert.IsTrue(actualResult.advancedInfo != null, "Expected valid result");
            Assert.IsFalse(actualResult.Result, "The test image should not be a match");

            var score = actualResult.advancedInfo.First(x => string.Equals(x.Key, "score", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("0.000", score.Value, "score value");
        }

        /// <summary>
        /// Evaluate an image providing binary content inline to the request
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
                Assert.IsTrue(actualResult.advancedInfo != null, "Expected valid result");
                Assert.IsFalse(actualResult.Result, "The test image should not be a match");

                var score =
                    actualResult.advancedInfo.First(
                        x => string.Equals(x.Key, "score", StringComparison.OrdinalIgnoreCase));
                double scoreValue = double.Parse(score.Value);
                Assert.IsTrue(scoreValue > 0, "Expected higher than 0 score value for test image");
            }
        }

        /// <summary>
        /// Add Image to image list, then verify it is matched
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
        /// Screen text against the default list of terms for english
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
            Assert.AreEqual("freaking", matchFlag.Source, "Expected term to match");
        }

        /// <summary>
        /// Add a term to the list, and validate we are able to match to it
        /// </summary>
        [TestMethod]
        public void AddTermTest()
        {
            IModeratorService moderatorService = new ModeratorService(this.serviceOptions);

            TextModeratableContent textContent = new TextModeratableContent("FakeProfanity");
            var taskResult = moderatorService.AddTermAsync(textContent, "eng");
            var actualResult = taskResult.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result for AddTerm");

            var refreshTask = moderatorService.RefreshTextIndexAsync("eng");
            var refreshResult = refreshTask.Result;
            Assert.IsTrue(refreshResult != null, "Expected valid result for RefreshIndex");

            var screenResponse = moderatorService.ScreenTextAsync(textContent, "eng");
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
        /// Identify the language of a text
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
