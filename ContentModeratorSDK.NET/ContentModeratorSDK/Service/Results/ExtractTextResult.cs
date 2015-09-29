using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service.Results
{
    public class ExtractTextResult : BaseModeratorResult
    {
        /// <summary>
        /// Array of name value pairs wit specific information about evaluate
        /// </summary>
        public AdvancedInfo[] AdvancedInfo;
    }
}
