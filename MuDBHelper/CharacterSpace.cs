using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuDBHelper
{
    class CharacterSpace : StorageSpace
    {
        public override int SpaceSize
        {
            get
            {
                return Storage.CHARACTER_SLOTS * Storage.ITEM_SIZE;
            }
        }
    }
}
