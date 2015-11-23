using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuDBHelper
{
    class ExpInventorySpace : StorageSpace
    {
        public override int SpaceSize
        {
            get
            {
                return Storage.EXP_INVENTORY_SLOTS * Storage.ITEM_SIZE;
            }
        }

        public ExpInventorySpace() : base()
        {

        }

        public ExpInventorySpace(string hex) : base(hex)
        {

        }
    }
}
