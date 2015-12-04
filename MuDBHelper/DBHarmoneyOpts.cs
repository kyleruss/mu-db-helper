using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace MuDBHelper
{
    [Table(Name="DBHelper_HarmoneyOptions")]
    class DBHarmoneyOpts
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID { get; set; }

        [Column]
        public string name { get; set; }

        [Column]
        public int type { get; set; }

        public override string ToString()
        {
            return name;
        }
    }
}
