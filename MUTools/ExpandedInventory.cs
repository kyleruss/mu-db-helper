using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTools
{
    class ExpandedInventory : StorageSpace
    {
        protected int space_size = 2048;

        public ExpandedInventory() : base()
        {

        }

        public ExpandedInventory(string hex) : base(hex)
        {

        }
    }
}
