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

namespace MuDBHelper
{
    public partial class MainWindow : Window
    {
        private DBItems current_item;
        private DBItemCategories current_category;

        public MainWindow()
        {
            InitializeComponent();
            fill_categories();
            InventorySpace inven = new InventorySpace();
            Item item = new Item(0, 4, true, true, 15, 4, 255, new ExcOpts(true, true, true, true, true, true));
            inven.addItem(item, 0);
            inven.buildSpaceHex();
            InventoryStorage storage = new InventoryStorage();
            storage.inventory = inven;
            storage.buildHex();
            Debug.WriteLine("HEX: " + storage.getBuiltHex());
        }

        public void fill_categories()
        {
            using (DBConnection conn = new DBConnection())
            {
                var categories = from e in conn.categories
                                 select e;
                items_list.ItemsSource = categories.ToList();
            }
        }

        public void getItemList(int category_id)
        {
            using (DBConnection conn = new DBConnection())
            {
               // Debug.WriteLine("CATEGORY: " + category_id);
                var items = from e in conn.items
                            where e.category_ID == category_id
                            select e;
                var category    =   from e in conn.categories
                                    where e.ID == category_id
                                    select e;

                items_list.ItemsSource = items.ToList();
               // current_category = category.First();
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
            fill_categories();
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
    }
}
