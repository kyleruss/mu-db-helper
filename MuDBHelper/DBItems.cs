using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuDBHelper
{
    [Table(Name="DBHelper_Items")]
    class DBItems
    {
        
        [Column(IsPrimaryKey=true)]
        public int? ID { get; set; }

        [Column(IsPrimaryKey=true)]
        public int? category_ID { get; set; }

        [Column]
        public string name { get; set; }

        [Column]
        public int? width { get; set; }

        [Column]
        public int? height { get; set; }

        [Column]
        public int? durability { get; set; }

        [Column]
        public int? slot_id { get; set; }

        [Column]
        public int? exc_type_ID { get; set; }

        [Column]
        public bool skill { get; set; }

        [Column]
        public bool allow_option { get; set; }

        [Column]
        public int? set1 { get; set; }

        [Column]
        public int? set2 { get; set; }

        [Column]
        public int? set3 { get; set; }

        [Column]
        public bool allow_socket { get; set; }

        [Column]
        public int? harmoney_type { get; set; }

        [Column]
        public int? refine_type { get; set; }

        [Column]
        public string image_path { get; set; }

        [Column]
        public int? itemType { get; set; }


        public string toString()
        {
            return name;
        }


        public static DBItems findItem(int category, int index)
        {
            using(DBConnection conn = new DBConnection())
            {
                return conn.items.Where(i => i.category_ID == category && i.ID == index).FirstOrDefault();
            }
        }
    }
}
