using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service.Requests
{
    using ContentModeratorSDK.Image;
    public class DetectFaceRequest : BaseImageRequest
    {
        public DetectFaceRequest(ImageModeratableContent content)
            : base(content)
        {
        }
    }
}
