using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service.Requests
{
    using ContentModeratorSDK.Text;

    public class IdentifyLanguageRequest : BaseModeratorRequest
    {
        public string Content;
        
        /// <summary>
        /// Request to screen text, determining if the content violates any policy (adult content, etc...)
        /// </summary>
        /// <param name="content"></param>
        public IdentifyLanguageRequest(TextModeratableContent content)
        {
            this.Content = content.ContentAsString;
        }
    }
}
