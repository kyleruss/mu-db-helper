using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTools
{
    class PersonalShop : StorageSpace
    {
        protected int space_size = 2048;

        public PersonalShop() : base()
        {

        }

        public PersonalShop(string hex) : base(hex)
        {

        }
    }
}
