using System;
using System.Collections.Generic;
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
            item_hover_level.Content = level;
        }

        public void setItemAddLevel(int addLevel)
        {
            item_hover_alevel.Content = addLevel;
        }

        public void setItemLuck(bool hasLuck)
        {
            item_hover_luck.IsChecked = hasLuck;
        }

        public void setItemSkill(bool hasSkill)
        {
            item_hover_skill.IsChecked = hasSkill;
        }
    }
}
