using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service.Results
{
    public class ImageListResult : BaseModeratorResult
    {
        public string ContentSource { get; set; }

        public List<string> ImageIds { get; set; }
    }
}
