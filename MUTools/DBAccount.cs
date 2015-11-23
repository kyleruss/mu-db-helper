using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace MUTools
{
    [Table(Name="MEMB_INFO")]
    class DBAccount
    {
        [Column(IsPrimaryKey = true)]
        public string memb___id { get; set; }

        [Column(IsDbGenerated = true)]
        public int memb_guid { get; set; }

        [Column]
        public string memb__pwd { get; set; }

        [Column]
        public string memb_name { get; set; }

        [Column]
        public string mail_addr { get; set; }

        [Column]
        public char ctl1_code { get; set; }

        [Column]
        public string fpas_ques { get; set; }

        [Column]
        public string fpas_answ { get; set; }
    }
}
