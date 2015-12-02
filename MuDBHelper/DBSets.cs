using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace MuDBHelper
{
    [Table(Name="DBHelper_Sets")]
    class DBSets
    {
        [Column(IsPrimaryKey=true)]
        public int? ID { get; set; }

        [Column]
        public string name { get; set; }
    }
}
