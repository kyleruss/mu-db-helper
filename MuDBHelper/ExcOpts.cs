using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuDBHelper
{
    class ExcOpts
    {
        public const int E1 = 1;
        public const int E2 = 2;
        public const int E3 = 4;
        public const int E4 = 8;
        public const int E5 = 16;
        public const int E6 = 32;

        public const int MAX_OPTS = 63;

        public int[] opt_vals = { 1, 2, 4, 8, 16, 32 };
        public string[] opt_keys = { "C1", "C2", "C3", "C4", "C5", "C6" };
        private Dictionary<string, Boolean> options;
        private string code;

        public ExcOpts() : this(false, false, false, false, false, false) {}

        public ExcOpts(Boolean E1, Boolean E2, Boolean E3, Boolean E4, Boolean E5, Boolean E6)
        {
            options = new Dictionary<string, bool>();
            options[opt_keys[0]] = E1;
            options[opt_keys[1]] = E2;
            options[opt_keys[2]] = E3;
            options[opt_keys[3]] = E4;
            options[opt_keys[4]] = E5;
            options[opt_keys[5]] = E6;
            calculateHexCode();
        }
        public ExcOpts(string code)
        {
            this.code = code;
            options = new Dictionary<string, Boolean>();
            foreach (string key in opt_keys)
                options[key] = false;
            calculateOptions();
        }

        public ExcOpts(Dictionary<string, Boolean> options)
        {
            this.options = options;
        }

        public Dictionary<string, Boolean> getOpts()
        {
            return options;
        }

        public bool isExcellent(int index)
        {
            if (index < 0 || index > opt_keys.Length) return false;
            else return options[opt_keys[index]];
        }

        public long[] calculateOptions()
        {
            long dec = Item.hexToDec(code);
            long bin = Item.parseBinary(dec);
            long[] temp = Item.toArray(bin);

            int j = 0;
            for (int i = temp.Length - 1; i >= 0; i--)
            {
                Boolean isOpt = false;
                if (temp[i] == 1) isOpt = true;
                options[options.ElementAt(j).Key] = isOpt;
                j++;
            }

            return temp;
        }

        public void updateHex()
        {
            code = "";
            int dec = 0;
            for (int i = 0; i < opt_keys.Length; i++)
                if (options[opt_keys[i]] == true) dec += opt_vals[i];
            code = Item.decToHex(dec);
        }

        public void changeOption(string key, Boolean val)
        {
            if (!opt_keys.Contains(key)) return;
            else
            {
                if(options[key] == val) return;
                else
                    options[key] = val;
                updateHex();
            }
        }

        public int calculateDecCode()
        {
            int dec = 0;
            for (int i = 0; i < opt_vals.Length; i++)
            {
                if (options.ElementAt(i).Value)
                    dec += opt_vals[i];
            }
            return dec;
        }

        public void calculateHexCode()
        {
            int decValue    =   calculateDecCode();
            code = Item.decToHex(decValue);
        }

        public string getCode()
        {
            return code;
        }
    }
        
}
