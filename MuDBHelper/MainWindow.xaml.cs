using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Xceed.Wpf.Toolkit;

namespace MuDBHelper
{
    public partial class MainWindow : Window
    {
        private DBItems current_item;
        private DBItemCategories current_category;
        private Dictionary<object, int> slot_indexes;

        public MainWindow()
        {
            InitializeComponent();
            fillCategories();
            fillLevelList();
            fillAddLevelList();
            initSlotIndexes();
        }

        private void fillCategories()
        {
            using (DBConnection conn = new DBConnection())
            {
                var categories = from e in conn.categories
                                 select e;
                items_list.ItemsSource = categories.ToList();
            }
        }

        private void fillLevelList()
        {
            List<string> levels = new List<string>();
            for(int i = 0; i < 16; i++)
                levels.Add("Level " + i);

            level_field.ItemsSource = levels;
            level_field.SelectedIndex = 0;
        }


        private void fillAddLevelList()
        {
            List<string> addLevels = new List<string>();
            for (int i = 0; i <= 28; i += 4)
                addLevels.Add("Level " + i);

            add_level_field.ItemsSource = addLevels;
            add_level_field.SelectedIndex = 0;
        }

        private void initSlotIndexes()
        {
            slot_indexes = new Dictionary<object, int>();
            slot_indexes.Add(slot_lweapon, 0);
            slot_indexes.Add(slot_rweapon, 1);
            slot_indexes.Add(slot_helm, 2);
            slot_indexes.Add(slot_chest, 3);
            slot_indexes.Add(slot_legs, 4);
            slot_indexes.Add(slot_gloves, 5);
            slot_indexes.Add(slot_boots, 6);
            slot_indexes.Add(slot_wings, 7);
            slot_indexes.Add(slot_pet, 8);
            slot_indexes.Add(slot_pen, 9);
            slot_indexes.Add(slot_lring, 10);
            slot_indexes.Add(slot_rring, 11);
        }

        public void getItemList(int category_id)
        {
            using (DBConnection conn = new DBConnection())
            {
                var items = from e in conn.items
                            where e.category_ID == category_id
                            select e;
                var category    =   from e in conn.categories
                                    where e.ID == category_id
                                    select e;

                items_list.ItemsSource = items.ToList();
            }
        }


        private void ItemListOnSelect(object sender, SelectionChangedEventArgs e)
        {
            if (items_list.SelectedItem != null)
            {
                if (current_category == null)
                {
                    current_category =  (DBItemCategories)items_list.SelectedItems[0];
                    category_label.Content = current_category.name;
                    getItemList(current_category.ID);

                    int excType = getExcType(current_category.ID, 0);
                    showExcOptions(excType);

                    categoryBackButton.Visibility = Visibility.Visible;
                }

                else
                {
                    current_item = (DBItems)items_list.SelectedItem;
                    item_image_name_label.Content = current_item.name;
                    current_item_image.Source = new BitmapImage(new Uri(@"/images/items/" + current_item.image_path, UriKind.Relative));
                }
            }

            if(current_category == null) items_list.UnselectAll();
           
        }

        private void CategoryBackOnClick(object sender, RoutedEventArgs e)
        {
            categoryBackButton.Visibility = Visibility.Hidden;
            current_category = null;
            fillCategories();
            showExcOptions(0);
        }

        private void showAccounts(string searchAccount)
        {
            using (DBConnection conn = new DBConnection())
            {
                var accounts = conn.accounts;

                if (searchAccount != null)
                    account_list.ItemsSource = accounts.Where(account => account.memb___id.Contains(searchAccount)).Select(account => account.memb___id);
                else
                    account_list.ItemsSource = accounts.Select(account => account.memb___id);
            }
        }

        private void showCharacters(string searchCharacter)
        {
            using (DBConnection conn = new DBConnection())
            {
                if (account_list.SelectedItem == null)
                {
                    var list = conn.characters;

                    if (searchCharacter != null)
                        character_list.ItemsSource = list.Where(character => character.Name.Contains(searchCharacter)).Select(character => character.Name);
                    else
                        character_list.ItemsSource = list.Select(character => character.Name);
                }

                else
                {
                    if (searchCharacter != null)
                    {
                        character_list.ItemsSource = conn.characters
                                                    .Where(character => character.AccountID == account_list.SelectedItem)
                                                    .Where(character => character.Name.Contains(searchCharacter))
                                                    .Select(character => character.Name);
                    }

                    else
                    {
                        character_list.ItemsSource = conn.characters
                            .Where(character => character.AccountID == account_list.SelectedItem)
                            .Select(character => character.Name);
                    }
                }
            }
        }

        private void AccountListOnOpen(object sender, EventArgs e)
        {
            showAccounts(null);
        }

        private void CharacterListOnOpen(object sender, EventArgs e)
        {
            showCharacters(null);
        }

        private void AccountSearchKeyUp(object sender, KeyEventArgs e)
        {
            showAccounts(account_list.Text);
        }

        private void CharacterSearchKeyUp(object sender, KeyEventArgs e)
        {
            showCharacters(character_list.Text);
        }

        private void showExcOptions(int type)
        {
            using (DBConnection conn = new DBConnection())
            {
                var opts = from e in conn.excOptions
                           where e.ID == type
                           select new 
                           { 
                               opt1 = e.option1, 
                               opt2 = e.option2,
                               opt3 = e.option3,
                               opt4 = e.option4,
                               opt5 = e.option5,
                               opt6 = e.option6
                           };

                var excOption = opts.First();
                exc_opt1.Content = excOption.opt1;
                exc_opt2.Content = excOption.opt2;
                exc_opt3.Content = excOption.opt3;
                exc_opt4.Content = excOption.opt4;
                exc_opt5.Content = excOption.opt5;
                exc_opt6.Content = excOption.opt6;
            }
        }

        private int getExcType(int category, int index)
        {
            if (category >= 0 && category < 6) return 1;

            else if (category >= 6 && category < 12) return 2;

            else return 0;
        }

        private ExcOpts getExcOpts()
        {
            return new ExcOpts((bool) exc_opt1.IsChecked, (bool) exc_opt2.IsChecked, (bool) exc_opt3.IsChecked, 
                               (bool) exc_opt4.IsChecked, (bool) exc_opt5.IsChecked, (bool) exc_opt6.IsChecked);
        }

        private Item getItem()
        {
            if (current_item == null) return null;
            else
            {
                int category = (int) current_item.category_ID;
                int index = (int) current_item.ID;
                int level = level_field.SelectedIndex;
                int add_level = add_level_field.SelectedIndex;
                int dur = int.Parse(dur_field.Text);
                bool luck = (bool) luck_field.IsChecked;
                bool skill = (bool) skill_field.IsChecked;
                ExcOpts excOpts = getExcOpts();

                return new Item(index, category, skill, luck, level, add_level, dur, excOpts);
            }
        }

        private void CharacterSlotOnClick(object sender, RoutedEventArgs e)
        {
            if (current_item == null) return;

            ImageBrush iBrush = new ImageBrush();
            iBrush.ImageSource = new BitmapImage(new Uri(current_item_image.Source.ToString()));
            ((Button) sender).Background = iBrush;

            int slotIndex = slot_indexes[sender];

           CharacterSpace inven = new CharacterSpace();
            Item item = getItem();
            inven.addItem(item, slotIndex);
            inven.buildSpaceHex();
            InventoryStorage storage = new InventoryStorage();
            storage.character = inven;
            string user = (string) character_list.SelectedItem;
            storage.saveCharacterInventory(user); 
        }
    }
}
