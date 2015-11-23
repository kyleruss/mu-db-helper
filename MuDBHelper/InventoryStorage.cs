using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuDBHelper
{
    class InventoryStorage
    {
        public CharacterSpace character { get; set; }

        public InventorySpace inventory { get; set; }

        public PShopSpace pShop { get; set; }

        public ExpInventorySpace expInventory { get; set; }

        public InventoryStorage()
        {
            inventory       =   new InventorySpace();
            pShop           =   new PShopSpace();
            expInventory    =   new ExpInventorySpace();
            character       =   new CharacterSpace();
        }

        public void buildHex()
        {
            character.buildSpaceHex();
            inventory.buildSpaceHex();
            pShop.buildSpaceHex();
            expInventory.buildSpaceHex();
        }

        public string getBuiltHex()
        {
            string hexCombined = character.getHex() + inventory.getHex() + pShop.getHex() + expInventory.getHex();
            int combinedLen = hexCombined.Length;
            int maxLen = 7584;
            string padding = new string('F', maxLen - combinedLen);
            return "0x" + hexCombined + padding;
        }
    }
}
