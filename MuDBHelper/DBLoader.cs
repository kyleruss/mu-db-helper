using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MuDBHelper
{
    class DBLoader
    {
        public static void buildDBItems(string file)
        {
            if (file == null) return;
            else
            {
                using (StreamReader reader = new StreamReader(file))
                {
                    string current_line = reader.ReadLine();
                    string[] item;
                    string regex = @"[ ](?=(?:[^""]*""[^""]*"")*[^""]*$)";
                    using (SqlConnection conn = new SqlConnection("Server=.\\SQLEXPRESS;Database=MuOnline;Integrated Security=true"))
                    {
                        conn.Open();
                        int current_category = 0;
                        int error_count = 0;
                        int line_number =   0;
                        string error_str = "";
                        do
                        {
                            line_number++;
                            //ignore commented and abnormal lines 
                            if (current_line.Length >= 2 && current_line.Substring(0, 2).Equals("//") || (current_line.Length >= 3 && (current_line.Substring(0, 3).Equals("end", StringComparison.InvariantCultureIgnoreCase))) || current_line.Equals("") || current_line.Equals(" "))
                                current_line = reader.ReadLine();


                            else if (current_line.Length < 3)
                            {
                                try  { int.TryParse(current_line.Substring(0, (current_line.Length == 2) ? 2 : 1), out current_category); }
                                catch(Exception e)
                                {
                                    error_count++;
                                    error_str += "[FORMAT ERROR] \nON LINE (" + line_number +  "): " + current_line;
                                }

                                current_line = reader.ReadLine();
                            }


                            else
                            {
                                using (SqlCommand command = new SqlCommand("DBHelper_InitItems", conn))
                                {
                                    command.CommandType = CommandType.StoredProcedure;
                                    item = Regex.Split(System.Text.RegularExpressions.Regex.Replace(current_line, @"\s+", " "), regex, RegexOptions.Multiline);
                                    Debug.WriteLine("LINE: " + current_line);
                                    try
                                    {
                                        command.Parameters.Add("@index", SqlDbType.Int).Value = int.Parse(item[0]);
                                        command.Parameters.Add("@category", SqlDbType.Int).Value = current_category;
                                        command.Parameters.Add("@name", SqlDbType.VarChar).Value = item[8].Replace("\"", "");
                                        command.Parameters.Add("@x", SqlDbType.Int).Value = int.Parse(item[3]);
                                        command.Parameters.Add("@y", SqlDbType.Int).Value = int.Parse(item[4]);
                                        command.Parameters.Add("@dur", SqlDbType.Int).Value = (current_category == 14) ? 1 : int.Parse(item[13]);
                                        command.Parameters.Add("@slot", SqlDbType.Int).Value = int.Parse(item[1]);
                                        command.Parameters.Add("@skill", SqlDbType.Bit).Value = (int.Parse(item[2]) != 0) ? 1 : 0;
                                        command.Parameters.Add("@allow_opt", SqlDbType.Bit).Value = int.Parse(item[6]);
                                        command.Parameters.Add("@allow_sock", SqlDbType.Bit).Value = 0;
                                    }

                                    catch (IndexOutOfRangeException e)
                                    {
                                        error_count++;
                                        error_str += "[FORMAT ERROR] \nON LINE (" + line_number +  "): " + current_line;
                                    }

                                    try { command.ExecuteNonQuery(); }
                                    catch (SqlException e)
                                    {
                                        error_count++;
                                        Debug.WriteLine(e.Message);
                                        error_str += "[SQL ERROR] \n--STACK TRACE: " + e.StackTrace + "\nON LINE (" + line_number +  "): " + current_line;
                                    } 

                                    current_line = reader.ReadLine();
                                }
                            }
                        }

                        while (current_line != null);

                        Console.WriteLine("Finished adding item(s) with " + error_count + " errors");
                        if (error_count > 0)
                        {
                            Console.WriteLine("------------------------------");
                            Console.WriteLine("ERROR LOG");
                            Console.WriteLine("------------------------------");
                            Console.WriteLine(error_str);
                        }
                    }

                }
            }
        }

    public static string calcItemImage(int theid, int type, int ExclAnci, int lvl)
    {
        string tnpl =   "";
        switch (ExclAnci) 
        {
            case 1:
                tnpl = "10";
                break;
            case 2:
                tnpl = "01";
                break;
            default:
                tnpl = "00";
                break;
        }

        int itype = type * 16;
        string nxt ="";
        string tipaj = "";
        if (theid > 63) 
            nxt = Item.decToHex(theid);

        else if (theid > 31) 
        {
            nxt = "F9";
            theid -= 32;
        } 
        
        else 
            nxt = "00";
    
        if (itype < 128) 
            tipaj = "00";

        else
        {
            tipaj = "80";
            itype -= 128;
        }
    
        theid += itype;
        itype += theid;
        string itype_new = itype.ToString("X2");
        string output="";

        if (File.Exists("images/items/" + tnpl + itype_new + tipaj + nxt + ".gif"))
            output = "" + tnpl + itype_new + tipaj + nxt + ".gif";

        else if (File.Exists("images/items/00" + tnpl + itype_new + tipaj + nxt + ".gif"))
            output = "00" + tnpl + itype_new + tipaj + nxt + ".gif";

        else
            output = null;

        return output;
    }


    public static void UpdateItemImages(Boolean reset)
    {
        using (DBConnection conn = new DBConnection())
        {
            var items = from e in conn.items
                        select e;
            foreach (var item in items)
            {
                if (item.image_path == null || item.image_path.Equals("default.gif") || reset)
                {
                    DBItems item_temp = item;
                    string path = calcItemImage((int)item.ID, (int)item.category_ID, 0, 0);
                    if (path == null) continue;
                    else
                    {
                        item_temp.image_path = path;
                        conn.SubmitChanges();
                    }
                }
            }
        }
    }

    public static void updateSingleItemImage(int item_id, int category_id, string path)
    {
        using (DBConnection conn = new DBConnection())
        {
            var item = from e in conn.items
                       where e.ID == item_id && e.category_ID == category_id
                       select e;
            
            DBItems item_temp = item.First();
            if (File.Exists("images/items/" + path))
            {
                item_temp.image_path = path;
                conn.SubmitChanges();
            }

        }
    }



        public void printItems(string[] item_arr)
        {
            foreach (string item in item_arr)
                Console.Write("[" + item + "] ");
        }

    }
}
