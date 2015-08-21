using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Result from screening a text, containing details regarding the Match
    /// </summary>
    public class IdentifyLanguageResult : BaseModeratorResult
    {
        public string DetectedLanguage;
    }
}
