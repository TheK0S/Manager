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
        bool isShowSuccessfulOperations = true;
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

            categoryGrid.ItemsSource = Data.GetCategories();
            wordCategory.ItemsSource = Data.GetCategories();
            langLevel.ItemsSource = levels;             

        }

        private void addCategory_Click(object sender, RoutedEventArgs e)
        {
            if(langLevel.SelectedItem != null)
            {
                Level level = (Level)langLevel.SelectedItem;
                if(categoryNameField.Text?.Length > 0)
                {
                    Data.CreateCategory(new Category { Id = 0, LevelsId = level.Id, CategoriesName = categoryNameField.Text }, isShowSuccessfulOperations);
                    categoryGrid.ItemsSource = Data.GetCategories();
                }
                else
                {
                    MessageBox.Show("Введите имя категории", "Пустое поле имени категории",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }            
            else
            {
                MessageBox.Show("Виберите уровень языка в списке и повторите попытку", "Не выбран уровень языка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addWord_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void updateCategory_Click(object sender, RoutedEventArgs e)
        {
            if(categoryGrid.SelectedItem != null)
            {
                Category category = (Category)categoryGrid.SelectedItem;

                foreach (var item in categories)
                {
                    if(category.Id == item.Id)
                    {
                        if(category.LevelsId == item.LevelsId && category.CategoriesName == item.CategoriesName)
                        {
                            MessageBox.Show("Не требуется изменение категории, так как данные категории в таблице не изменились");
                        }
                        else
                        {
                            Data.UpdateCategory(category, item, isShowSuccessfulOperations);
                            break;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Виберите категорию в таблице и повторите попытку", "Не выбрана категория",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void removeCategory_Click(object sender, RoutedEventArgs e)
        {
            if(categoryGrid.SelectedItem != null)
            {
                Data.RemoveCategory((Category)categoryGrid.SelectedItem, isShowSuccessfulOperations);
                categoryGrid.ItemsSource = Data.GetCategories();
                categories = Data.GetCategories();
            }
            else
            {
                MessageBox.Show("Виберите категорию в таблице и повторите попытку", "Не выбрана категория для удаления",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void updateWord_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeWord_Click(object sender, RoutedEventArgs e)
        {
            if (wordGrid.SelectedItem != null)
            {
                Word word = (Word)wordGrid.SelectedItem;
                Data.RemoveWord(word, isShowSuccessfulOperations);

                if (word.CategoryName != null)
                    wordTables[word.CategoryName] = Data.GetWords(word.CategoryName);
            }
            else
            {
                MessageBox.Show("Виберите слово в таблице и повторите попытку", "Не выбрано слово для удаления", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void wordCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wordCategory.SelectedItem != null)
            {
                try
                {
                    Category category = (Category)wordCategory.SelectedItem;
                    if (category.CategoriesName != null)
                        wordGrid.ItemsSource = wordTables[category.CategoriesName];
                }
                catch (Exception)
                {
                    MessageBox.Show("После редактирования категории в таблице, не изменяйте выбор строки до нажатия кнопки \"Редактировать категорию\"",
                        "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void isShowSuccOperations_Unchecked(object sender, RoutedEventArgs e)
        {
            isShowSuccessfulOperations = false;
        }

        private void isShowSuccOperations_Checked(object sender, RoutedEventArgs e)
        {
            isShowSuccessfulOperations = true;
        }
    }
}
