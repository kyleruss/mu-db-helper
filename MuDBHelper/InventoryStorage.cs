using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

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

        public void saveCharacterInventory(string characterName)
        {
            string inventoryHex = getBuiltHex();
            Debug.WriteLine(inventoryHex);
            using(DBConnection conn = new DBConnection())
            {
                try
                {
                    string sql = String.Format("UPDATE Character SET Inventory={0} WHERE Name='{1}'", inventoryHex, characterName);
                    conn.ExecuteCommand(sql);
                }

                catch(Exception e)
                {
                    Debug.WriteLine("sql excp: " + e.Message);
                }
            }
        }
    }
}
