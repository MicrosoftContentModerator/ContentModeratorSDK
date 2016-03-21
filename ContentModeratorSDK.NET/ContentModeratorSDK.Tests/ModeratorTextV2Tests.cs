using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ContentModeratorSDK.Service;
using System.Configuration;
using ContentModeratorSDK.Text;
using System.Linq;
using Newtonsoft.Json;
using ContentModeratorSDK.Service.Results;
using ContentModeratorSDK.Tests.Helpers;

namespace ContentModeratorSDK.Tests
{
    [TestClass]    
    public class ModeratorTextV2Tests
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
               
              
                TextServiceCustomListPath = ConfigurationManager.AppSettings["TextServiceCustomListPath"],
                TextServicePathV2 = ConfigurationManager.AppSettings["TextServicePathV2"],
              

                // Input your keys after signing up for content moderator below
                // Visit the API manager portal to get keys:
                // https://developer.microsoftmoderator.com/docs/services?ref=mktg
               
                TextServiceKey = ConfigurationManager.AppSettings["TextServiceKey"],
                TextServiceCustomListKey = ConfigurationManager.AppSettings["TextServiceCustomListKey"]
               

            };           
        }

        

        /// <summary>
        /// Screen text against the default list of terms for english. Validate that the text is matched.
        /// </summary>
        [TestMethod]
        [TestCategory("Text.Screen")]
        public void ScreenTextTest()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

            // Import the term list. This needs to only be done once before screen
            moderatorService.ImportTermListAsync("eng").Wait();

            moderatorService.RefreshTextIndexAsync("eng").Wait();

            // Run screen to match, validating match result
            string text = "The <a href=\"www.bunnies.com\">qu!ck</a> brown  <a href=\"b.suspiciousdomain.com\">f0x</a> jumps over the lzay dog www.benign.net. freaking awesome.";
            TextModeratableContent textContent = new TextModeratableContent(text);
            var screenResponse = moderatorService.ScreenTextAsync(textContent, "eng");
            var screenResult = screenResponse.Result;

            var expectedScreenTextResult = JsonConvert.DeserializeObject<ScreenTextResult>(ResultJsonForComparison.TextV2_ScreenTextTest);

            Assert.IsTrue(screenResult != null, "Expected valid result");
            Assert.AreEqual(expectedScreenTextResult.NormalizedText, screenResult.NormalizedText);
            Assert.IsTrue(expectedScreenTextResult.Terms.SequenceEqual(screenResult.Terms, new TermMatchComparer()));
            Assert.IsTrue(expectedScreenTextResult.Terms.SequenceEqual(screenResult.Terms, new TermMatchComparer()));
            Assert.IsTrue(expectedScreenTextResult.Urls.SequenceEqual(screenResult.Urls, new URLMatchComparer()));

        }

       
        /// <summary>
        /// Add a term to the list, and validate we are able to match to it
        /// </summary>
        [TestMethod]
        [TestCategory("Text.CustomList")]
        public void AddTermTest()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

            // We are creating a term "FakeProfanity" in english (thus provide tha same english translation), then matching against it.
            TextModeratableContent textContent = new TextModeratableContent(text: "ertuythfg", englishTranslation: "FakeProfanity");
            var taskResult = moderatorService.AddTermAsync(textContent, "eng");

            var actualResult = taskResult.Result;
            Assert.IsTrue((actualResult.StatusCode != System.Net.HttpStatusCode.Created) || (actualResult.StatusCode != System.Net.HttpStatusCode.MultipleChoices), "Expected valid result for AddTerm");

            var refreshTask = moderatorService.RefreshTextIndexAsync("eng");
            var refreshResult = refreshTask.Result;
            Assert.IsTrue(refreshResult != null, "Expected valid result for RefreshIndex");

            var screenResponse = moderatorService.ScreenTextAsync(new TextModeratableContent("This is a ertuythfg!"), "eng");
            var screenResult = screenResponse.Result;           
            

            var deleteTask = moderatorService.RemoveTermAsync(textContent, "eng");
            var deleteResult = deleteTask.Result;
            Assert.IsTrue(deleteResult.IsSuccessStatusCode, "Expected valid result for DeleteTerm");
        }

        /// <summary>
        /// Add a term to the list, and validate we are able to match to it
        /// </summary>
        [TestMethod]
        [TestCategory("Text.CustomList")]
        public void RemoveAllTermsInALanguageTest()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            var deleteTask = moderatorService.RemoveAllTermsAsync("eng");
            var deleteResult = deleteTask.Result;
            Assert.IsTrue(deleteResult.IsSuccessStatusCode, "Expected valid result for DeleteTerm");
        }

        [TestMethod]
        [TestCategory("Text.CustomList")]
        public void GetTermListTest()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);
            var taskResult = moderatorService.ListTermsAsync("eng");
            var actualResult = taskResult.Result;
            
            Assert.IsTrue(actualResult != null, "Expected valid result for Get Term List");            
            Assert.IsTrue(actualResult.Terms.Count>0, "The count of terms is 0");
        }
        /// <summary>
        /// Identify the language of an input text
        /// </summary>
        [TestMethod]        
        [TestCategory("Text.DetectLanguage")]
        public void IdentifyLanguageTest()
        {
            IModeratorServiceV2 moderatorService = new ModeratorServiceV2(this.serviceOptions);

            TextModeratableContent textContent = new TextModeratableContent("Hola este es un texto en otro idioma");
            var identifyLanguageResponse = moderatorService.IdentifyLanguageAsync(textContent);
            var actualResult = identifyLanguageResponse.Result;
            Assert.IsTrue(actualResult != null, "Expected valid result");
            Assert.AreEqual("spa", actualResult.DetectedLanguage, "Expected valid result");
        }
    }
}
