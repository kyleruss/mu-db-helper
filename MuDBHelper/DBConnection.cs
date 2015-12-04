using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace MuDBHelper
{
    class DBConnection : DataContext
    {
        public Table<DBItems> items;

        public Table<DBItemCategories> categories;

        public Table<DBAccount> accounts;

        public Table<DBCharacter> characters;

        public Table<DBExcOpts> excOptions;

        public Table<DBSets> ancSets;

        public Table<DBHarmoneyOpts> harmOpts;

        public DBConnection() : base("Server=.\\SQLEXPRESS;Database=MuOnline;Integrated Security=true") { }
    }
}
