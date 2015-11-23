using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace MUTools
{
    [Table(Name="Character")]
    class DBCharacter
    {
        [Column(IsPrimaryKey = true)]
        public string Name { get; set; }

        [Column]
        public string AccountID { get; set; }

        [Column]
        public int cLevel { get; set; }

        [Column]
        public short Class { get; set; }

        [Column]
        public long Experience { get; set; }

        [Column]
        public int Strength { get; set; }

        [Column]
        public int Dexterity { get; set; }

        [Column]
        public int Vitality { get; set; }

        [Column]
        public int Energy { get; set; }

        [Column]
        public int Leadership { get; set; }

        [Column]
        public int Money { get; set; }

        [Column]
        public long Life { get; set; }

        [Column]
        public long MaxLife { get; set; }

        [Column]
        public long Mana { get; set; }

        [Column]
        public long MaxMana { get; set; }

        [Column]
        public short MapNumber { get; set; }

        [Column]
        public int Resets { get; set; }

        [Column]
        public int GrandResets { get; set; }
    }
}
