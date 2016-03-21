using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentModeratorSDK.Service.Results
{
    public class TermListResult: BaseModeratorResult
    {
        public List<TermObj> Terms { get; set; }
    }

    public class TermObj
    {
        public string Term { get; set; }
    }
}
