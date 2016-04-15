using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbclass2.Objects
{
    public class AccessInfo
    {
        public AccessInfo()
        {

        }

        public string SourceUrl { get; set; }

        public string SourceUserName { get; set; }

        public string SourcePassword { get; set; }

        public string TargetUrl { get; set; }

        public string TargetUserName { get; set; }

        public string TargetPassword { get; set; }

    }
}
