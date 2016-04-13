using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dbclass2.Objects
{
    public class ColumnInfo
    {
        public ColumnInfo()
        {

        }

        public string Name { get; set; }

        public string DataType { get; set; }

        public string IsNull { get; set; }

        public string DataLength { get; set; }

        public string ConstraintType { get; set; }
    }
}
