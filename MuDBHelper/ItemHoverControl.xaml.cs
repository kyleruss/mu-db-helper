using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MuDBHelper
{
    /// <summary>
    /// Interaction logic for ItemHoverControl.xaml
    /// </summary>
    public partial class ItemHoverControl : UserControl
    {
        public ItemHoverControl()
        {
            InitializeComponent();
        }

        public void setItemName(string name)
        {
            item_hover_name.Content = name;
        }

        public void setItemLevel(int level)
        {
            item_hover_level.Content = "Level: +" + level;
        }

        public void setItemAddLevel(int addLevel)
        {
            item_hover_alevel.Content = "Add Level: +" + addLevel;
        }

        public void setItemLuck(bool hasLuck)
        {
            item_hover_luck.IsChecked = hasLuck;
        }

        public void setItemSkill(bool hasSkill)
        {
            item_hover_skill.IsChecked = hasSkill;
        }

        public void setItemExc(Item item)
        {
            Dictionary<string, bool> opts = item.excellent_options.getOpts();
            DBExcOpts exOpts = null;

            using(DBConnection conn = new DBConnection())
            {
                exOpts = conn.excOptions.Where(c => c.ID == item.getItemType()).First();
            }

            int index = 0;

            foreach(Label label in exc_opts_grid.Children.Cast<Label>())
            {
                if (opts.ElementAt(index).Value)
                {
                    if(exOpts != null)
                    {
                        switch(index)
                        {
                            case 0: label.Content = exOpts.option1; break;
                            case 1: label.Content = exOpts.option2; break;
                            case 2: label.Content = exOpts.option3; break;
                            case 3: label.Content = exOpts.option4; break;
                            case 4: label.Content = exOpts.option5; break;
                        }
                    }

                    label.Visibility = Visibility.Visible;
                }
                else
                    label.Visibility = Visibility.Hidden;

                index++;
            }
        }
    }
}
