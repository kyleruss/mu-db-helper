using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuDBHelper
{
    class PShopSpace : StorageSpace
    {
        public override int SpaceSize
        {
            get
            {
                return Storage.STORE_SLOTS * Storage.ITEM_SIZE;
            }
        }

        public PShopSpace() : base()
        {

        }

        public PShopSpace(string hex) : base(hex)
        {

        }
    }
}
