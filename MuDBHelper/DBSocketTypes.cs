using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace MuDBHelper
{
    [Table(Name="DBHelper_SocketTypes")]
    class DBSocketTypes
    {
        [Column(IsPrimaryKey=true, IsDbGenerated=true)]
        public int id { get; set; }

        [Column]
        public string level1_value { get; set; }

        [Column]
        public string level2_value { get; set; }

        [Column]
        public string level3_value { get; set; }

        [Column]
        public string level4_value { get; set; }

        [Column]
        public string level5_value { get; set; }
    }
}
