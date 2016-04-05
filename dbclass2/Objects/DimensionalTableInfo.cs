using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbclass2
{
    class DimensionalTableInfo
    {
        public DimensionalTableInfo()
        {

        }

        public string TableName { get; set; }

        public Dictionary<string, string> PrimaryKeys { get; set; }

        public Dictionary<string, string> Columns { get; set; }
    }
}
