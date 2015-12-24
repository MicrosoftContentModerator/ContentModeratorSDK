using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service.Results
{
    /// <summary>
    /// Result for faces detected
    /// </summary>
    public class DetectFaceResult : BaseModeratorResult
    {
        /// <summary>
        /// Advanced Information from the DetectFace
        /// </summary>
        public AdvancedInfo[] AdvancedInfo;

        /// <summary>
        /// Boolean indicating whether face are detected
        /// </summary>
        public bool Result;

        /// <summary>
        /// Faces detected
        /// </summary>
        public double Count;

        /// <summary>
        /// Face dimensions
        /// </summary>
        public Faces[] Faces;
    }

    /// <summary>
    /// Face dimensions
    /// </summary>
    public class Faces
    {
        /// <summary>
        /// Face bottom
        /// </summary>
        public double Bottom;

        /// <summary>
        /// Face Left
        /// </summary>
        public double Left;

        /// <summary>
        /// Face Right
        /// </summary>
        public double Right;

        /// <summary>
        /// Face Top.
        /// </summary>
        public double Top;
    }
}
