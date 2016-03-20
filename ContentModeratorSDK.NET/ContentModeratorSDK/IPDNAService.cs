using ContentModeratorSDK.Image;
using ContentModeratorSDK.Service.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK
{
    public interface IPDNAService
    {
        //PDNA Methods

        /// <summary>
        /// Validate an image against the images in the PDNA DB
        /// </summary>
        /// <param name="imageContent">Image to match</param>
        /// <param name="cacheContent">Cache Image content</param>
        /// <returns>Match response</returns>
        Task<MatchImageResult> ValidateImageAsync(ImageModeratableContent imageContent, bool cacheContent = false);

        /// <summary>
        /// Call Validate Image in Cache, against the images in the PDNA DB
        ///  on multiple ratings.
        /// </summary>
        /// <param name="cacheId">cached image id</param>
        /// <returns>Evaluate result</returns>
        Task<MatchImageResult> ValidateImageInCache(string cacheId);
    }
}
