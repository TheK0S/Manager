using Manager.Model;
using Manager.View;
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

namespace Manager
{
    /// <summary>
    /// Логика взаимодействия для DataPage.xaml
    /// </summary>
    public partial class DataPage : Page
    {
        List<Category> categories = new List<Category>();
        List<Level> levels = new List<Level>();
        Dictionary<string, List<Word>> wordTables = new Dictionary<string, List<Word>>();

        public DataPage()
        {
            InitializeComponent();

            categoryGrid.CanUserAddRows = false;
            wordGrid.CanUserAddRows = false;
            

            levels = Data.GetLevels();
            categories = Data.GetCategories();          

            
            foreach (var category in categories)
                wordTables.Add(category.CategoriesName, Data.GetWords(category.CategoriesName));

            categoryGrid.ItemsSource = categories;
            wordCategory.ItemsSource = categories;

            if (categories?.Count > 0)
            {              
                categoryGrid.SelectedItem = categories[0];
                wordGrid.ItemsSource = wordTables[categories[0].CategoriesName];
            }    

        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddCategory(in levels));
        }

        private void addWord_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddWord());
        }
    }
}
