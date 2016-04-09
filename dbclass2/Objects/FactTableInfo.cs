using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbclass2
{
    public class FactTableInfo : DimensionalTableInfo
    {
        public FactTableInfo()
        {

        }

        public Dictionary<string, string> Relations { get; set; }
    }
}
