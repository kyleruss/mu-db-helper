using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace MuDBHelper
{
    class StorageSpace
    {
        public virtual int SpaceSize
        {
            get { return 0; }
        }

        protected char[] hexContainer;
        public Item[] items { get; set; }
        protected string hex;

        public StorageSpace()
        {
            hex = new string('F', SpaceSize);
            items = new Item[SpaceSize / 32];
            initEmptyItems();
        }

        public StorageSpace(string hex)
        {
            this.hex = hex;
            initSpace();
            items = new Item[SpaceSize / 32];
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
            if (hex.Length < SpaceSize)
                hex += new string('F', SpaceSize - hex.Length);
            hexContainer = hex.ToCharArray();
        }

        public string getHex()
        {
            return hex;
        }
    }
}
