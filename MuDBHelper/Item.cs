using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuDBHelper
{
    class Item
    {
        public string EMPTY = new string('F', 32);
        public string EMPTY_LARGE = new string('F', 64);
        public string def = new string('0', 22) + new string('F', 10);
        public const int length = 32;
        private char[] item;
        private string hex;

        private int start;
        private int end;

        public ExcOpts excellent_options { get; private set; }
        public int durability { get; private set; }
        public int level { get; private set; }
        public int addLevel { get; private set; }
        public Boolean luck { get; private set; }
        public Boolean skill { get; private set; }
        public string serial { get; private set; }
        public int index { get; private set; }
        public int category { get; private set; }

        public Item()
        {
            hex = new string('F', 32);
            item = new char[hex.Length];
            initItem();
        }

        public Item(string hex, int start, int end) : this(hex)
        {
            this.start = start;
            this.end = end;
        }

        public Item(string hex)
        {
            item = new char[hex.Length];
            this.hex = hex;
            initItem();
            initByte2();
            initExcOpts();
        }
                                                                                                
        public Item(int index, int category, bool skill, bool luck, int level, int addLevel, int durability, ExcOpts excellent_options)
        {
            item     = new char[32];
            this.hex = def;
            initItem();
            changeIndex(index);
            changeCategory(category);
            changeLevel(level);
            changeSkill(skill);
            changeLuck(luck);
            changeAdditionalOpts(addLevel);
            setDurability(durability);
            initExcOptsFromObject(excellent_options.getCode(), excellent_options);
            initExcOptsToItem();
            updateHex();
        }

        public Boolean isEmpty()
        {
            return (hex == EMPTY || hex == EMPTY_LARGE);
        }

        public void initItem()
        {
            item = hex.ToCharArray();
        }

        public void initExcOpts()
        {
            string exc_hex = "" + item[14] + item[15];
            initExcOptsFromObject(exc_hex, null);
        }

        public void initExcOptsFromObject(string exc_hex, ExcOpts opts)
        {
            if (exc_hex.Equals("00") || exc_hex.Equals("FF")) excellent_options = new ExcOpts();
            else
            {
                if (addLevel > 3)
                {
                    int dec = (int)hexToDec(exc_hex.Substring(0, 1));
                    dec -= 4;
                    exc_hex = decToHex(dec).Substring(1, 1) + exc_hex.Substring(1, 1);
                }

                excellent_options = (opts == null) ? new ExcOpts(exc_hex) : opts;
            }
        }

        public void initExcOptsToItem()
        {
            if (excellent_options == null) return;
            else
            {
                string hex_str = excellent_options.getCode();

                int dec  = (int)hexToDec(hex_str.Substring(0, 1));
                int dec2 = (int)hexToDec(hex_str.Substring(1, 1));
                int res = (addLevel > 3) ? 4 : 0;

                dec += res;
                item[14] = char.Parse(decToHex(dec).Substring(1, 1));
                item[15] = char.Parse(decToHex(dec2).Substring(1, 1));
            }
        }

        public void initProperties()
        {
           initByte2();
           getDurabilityFromHex();
           setSerial();
        }

        public void updateHex()
        {
            hex = new string(item);
        }

        public string getHexString()
        {
            return hex;
        }

        public char[] getItemFull()
        {
            return item;
        }

        public void calculateIndex()
        {
            index = (int)hexToDec("" + item[0] + item[1]);
        }

        public void calculteCategory()
        {
            category = (int)hexToDec("" + item[18]);
        }

        public void changeIndex(int index_change)
        {
            if (index_change > 255 || index_change < 0) return;
            else
            {
                string index_hex = decToHex(index_change);
                item[0] = char.Parse(index_hex.Substring(0, 1));
                item[1] = char.Parse(index_hex.Substring(1, 1));
                index = index_change;
            }
        }

        public void changeCategory(int category_change)
        {
            if (category_change > 255 || category_change < 0) return;
            else
            {
                string cat_hex = decToHex(category_change);
                item[17] = char.Parse(cat_hex.Substring(0, 1));
                item[18] = char.Parse(cat_hex.Substring(1, 1));
                category = category_change;
            }
        }

        public void changeLuck(Boolean luck_changed)
        {
            if (luck_changed == luck) return;
            else
            {
                int dec = (int)hexToDec(item[3].ToString());
                dec += (luck_changed) ? 4 : -4;
                item[3] = char.Parse(decToHex(dec).Substring(1, 1));
                luck = true;
            }
        }

        public void changeSkill(Boolean skill_changed)
        {
            if (skill_changed == skill) return;
            else
            {
                int dec = (int)hexToDec(item[2].ToString());
                if (skill_changed) dec += 6;
                else dec -= 6;
                item[2] = char.Parse(decToHex(dec).Substring(1, 1));
                skill = true;
            }
        }

        public int getDurabilityFromHex()
        {
            return (int) hexToDec("" + item[4] + item[5]);
        }

        public void setDurability(int dur)
        {
            if (dur > 255 || dur < 0) return;
            else
            {
                string hex_dur = decToHex(dur);
                item[4] = char.Parse(hex_dur.Substring(0, 1));
                item[5] = char.Parse(hex_dur.Substring(1, 1));
                durability = dur;
            }
        }


        public void changeLevel(int new_level)
        {
            if (new_level > 15 || new_level < 0) return;
            else
            {
                int dec = (int)hexToDec(item[2].ToString() + item[3].ToString());
                if (new_level > level) dec += (8* (new_level - level));
                else dec -= (8* (level - new_level));
                level = dec / 8;

                string hex_temp = decToHex(dec);
                item[2] = char.Parse(hex_temp.Substring(0, 1));
                item[3] = char.Parse(hex_temp.Substring(1, 1));
            }
        }


        
        public void changeAdditionalOpts(int opt_level)
        {
            if (opt_level == addLevel || opt_level < 0 || opt_level > 7) return;
            else
            {
                int dec = (int)hexToDec(item[3].ToString());
                int dec2 = (int)hexToDec(item[14].ToString());

                if (addLevel == null)
                {
                    if (opt_level > 3)
                    {
                        dec2 += 4;
                        dec += (opt_level - 4);
                    }

                    else
                        dec += opt_level;
                }

                else
                {

                    if (opt_level > 3 && addLevel <= 3)
                    {
                        dec2 += 4;
                        int dec_temp = opt_level - 4;
                        dec -= addLevel;
                        dec += dec_temp;

                    }
                    else if (opt_level <= 3 && addLevel > 3)
                    {
                        dec2 -= 4;
                        int dec_temp = addLevel - 4;
                        dec -= dec_temp;
                        dec += opt_level;
                    }

                    else
                    {
                        int dec_temp = (addLevel > 3) ? addLevel - 4 : addLevel;
                        dec -= dec_temp;
                        dec += (opt_level > 3) ? opt_level - 4 : opt_level;
                    }
                }

                item[3] = char.Parse(decToHex(dec).Substring(1, 1));
                item[14] = char.Parse(decToHex(dec2).Substring(1, 1));
                addLevel = opt_level;
            }
        }


        //--------------------------------------------------------------
        //Skill, level, luck and additional damge properties calculated
        //--------------------------------------------------------------
        public void initByte2()
        {
            if (hex == null) return;
            else
            {
                int[] temp = { (int)hexToDec(hex.Substring(2, 1)), (int)hexToDec(hex.Substring(3, 1)) };
                int addOpt = (int)hexToDec(hex.Substring(14, 1));
                bool is_opt = false;

                //add opts adds +4 @ pos 14 if opts are >= +16
                if (addOpt >= 4) is_opt = true;
                //Determining +skill:
                if (temp[0] > 7)
                {
                    skill = true;
                    temp[0] -= 8;
                }

                //deal with level, luck and additional opts
                if (temp[1] < 4) luck = false;

                //item has luck and possibly additional opts
                else if (temp[1] >= 4 && temp[1] <= 7)
                {
                    luck = true;
                    temp[1] -= 4;
                    addLevel = temp[1] + addOpt;
                    temp[1] = 0;
                    level = (int)hexToDec("" + temp[0] + temp[1]) / 8;
                }

                //item has luck additional opts and level > 0
                else if (temp[1] >= 12 && temp[1] <= 15)
                {
                    luck = true;
                    temp[1] -= 4;
                    temp[1] -= 8; //temp remove level component to find add_level. Add back later.
                    addLevel = temp[1] + addOpt;
                    temp[1] = 8;
                    level = (int)hexToDec("" + temp[0] + temp[1]) / 8;
                }
            }
        }

        public string generateSerial()
        {
            int dec = 0; // call WZ_GetItemSerial 1 and fetch new item count
            return String.Format("{0:X8}", dec);
        }

        public string getSerialFromHex()
        {
            String serial = "";
            for (int i = 6; i <= 13; i++)
                serial += item[i];
            return serial;
        }

        public void setSerial()
        {
            serial = getSerialFromHex();
        }

        public static string decToHex(int dec)
        {
            string hex_str = ((dec < 16) ? "0" : "") + dec.ToString("X");
            return hex_str;
        }

        public static long hexToDec(string hex)
        {
            return Convert.ToInt64(hex, 16);
        } 

        public static long parseBinary(long n)
        {
            return int.Parse(Convert.ToString(n, 2));
        }

        public static long[] toArray(long n)
        {
            string n2 = n.ToString();
            long[] temp = new long[n2.Length];
            for (int i = 0; i < n2.Length; i++)
                temp[i] = int.Parse(n2.Substring(i, 1));
            return temp;
        }

        public string getItemCode()
        {
            string hex_code = "";
            foreach (char index in item)
                hex_code += index;
            return hex_code;
        }
    }
}
