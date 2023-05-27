using Manager.Model;
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

            if(categories != null )
                foreach (var category in categories)
                    wordTables.Add(category.CategoriesName, Data.GetWords(category.CategoriesName));

            categoryGrid.ItemsSource = categories;
            wordCategory.ItemsSource = categories;
            langLevel.ItemsSource = levels;             

        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void addWord_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void updateCategory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeCategory_Click(object sender, RoutedEventArgs e)
        {
            Data.RemoveCategory((Category) categoryGrid.SelectedItem);
            categories = Data.GetCategories();
        }

        private void updateWord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeWord_Click(object sender, RoutedEventArgs e)
        {
            Word word = (Word) wordGrid.SelectedItem;
            Data.RemoveWord(word);

            if (word.CategoryName != null)
                wordTables[word.CategoryName] = Data.GetWords(word.CategoryName);
        }

        private void wordCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wordCategory.SelectedItem != null)
            {
                Category category = (Category)wordCategory.SelectedItem;
                if (category.CategoriesName != null)
                    wordGrid.ItemsSource = wordTables[category.CategoriesName];
            }                
        }
    }
}
