using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuDBHelper
{
    class InventorySpace : StorageSpace
    {
        public override int SpaceSize
        {
            get
            {
                return Storage.INVENTORY_SLOTS * Storage.ITEM_SIZE;
            }
        }

        public InventorySpace() : base()
        {
            
        }

        public InventorySpace(string hex) : base(hex)
        {

        }
    }
}
