using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTools
{
    class Inventory
    {
        public const int INV_SIZE = 2048;
        public string DEF_INV_SPACE = new string('F', INV_SIZE);
        private char[] inventoryHolder;
        private Item[] items;
        private string hex;

        public Inventory()
        {
            hex = DEF_INV_SPACE;
            items = new Item[INV_SIZE / 32];
            initBlankItems();
        }

        public Inventory(string hex)
        {
            this.hex = hex;
            initInventory();
            items = new Item[INV_SIZE / 32];
            buildItemsFromHex();
        }

        public void buildItemsFromHex()
        {
            if (hex == null || hex == "") return;
            else
            {
                for (int i = 0; i < items.Length; i++)
                {
                    string item = hex.Substring(i * 32, 32);
                    items[i] = (item.Equals(new string('F', 32))) ? new Item() : new Item(item);
                }

            }
        }

        public void buildInventoryHex()
        {
            string hex_temp = "";
            foreach (Item item in items)
                hex_temp += item.getHexString();
            hex = hex_temp;
        }

        public void addItem(Item item, int index)
        {
            Item temp = items[index];
            if (!temp.isEmpty()) return;
            else
                items[index] = item;
        }

        public void initBlankItems()
        {
            for (int i = 0; i < items.Length; i++)
                items[i] = new Item();
        }

        public void initInventory()
        {
            if (hex.Length < INV_SIZE) 
                hex += new string('F', INV_SIZE - hex.Length);
            inventoryHolder = hex.ToCharArray();
        }

        public string getHex()
        {
            return hex;
        }
    }
}
