using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace MuDBHelper
{
    [Table(Name = "[DBHelper_ExcTypes]")]
    class DBExcOpts
    {
        [Column(IsPrimaryKey = true, IsDbGenerated=true)]
        public int ID { get; set; }

        [Column]
        public string type_name { get; set; }

        [Column]
        public string option1 { get; set; }

        [Column]
        public string option2 { get; set; }

        [Column]
        public string option3 { get; set; }

        [Column]
        public string option4 { get; set; }

        [Column]
        public string option5 { get; set; }

        [Column]
        public string option6 { get; set; }
    }
}
