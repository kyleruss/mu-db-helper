using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MUTools
{
    class StorageSpace
    {
        protected int space_size = 0;
        protected char[] hexContainer;
        protected Item[] items;
        protected string hex;

        public StorageSpace()
        {
            hex = new string('F', space_size);
            items = new Item[space_size / 32];
            initEmptyItems();
        }

        public StorageSpace(string hex)
        {
            this.hex = hex;
            initSpace();
            items = new Item[space_size / 32];
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

        public void buildSpaceHex()
        {
            string hex_temp = "";
            foreach (Item item in items)
                hex_temp += item.getHexString();
            hex = hex_temp;
        }

        public void addItem(Item item, int index)
        {
            Item temp = items[index];
            if (!temp.isEmpty()) 
                return;

            else
                items[index] = item;
        }

        public void initEmptyItems()
        {
            for (int i = 0; i < items.Length; i++)
                items[i] = new Item();
        }

        public void initSpace()
        {
            if (hex.Length < space_size)
                hex += new string('F', space_size - hex.Length);
            hexContainer = hex.ToCharArray();
        }

        public string getHex()
        {
            return hex;
        }
    }
}
