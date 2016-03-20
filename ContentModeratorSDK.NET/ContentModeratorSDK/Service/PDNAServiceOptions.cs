using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service
{
    public class PDNAServiceOptions
    {
        /// <summary>
        /// Url of host
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// Url Path to PDNA image service
        /// </summary>
        public string PDNAImageServicePath { get; set; }

        /// <summary>
        /// Key for PDNA image service
        /// </summary>
        public string PDNAImageServiceKey { get; set; }
    }
}
